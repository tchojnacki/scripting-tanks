using System.Net.WebSockets;
using Backend.Domain;
using Backend.Rooms;
using Backend.Identifiers;
using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Server;
using Backend.Contracts.Messages.Client;
using Backend.Utils.Mappings;

namespace Backend.Services;

public class ConnectionManager : IConnectionManager
{
    private readonly Dictionary<CID, PlayerData> _activeConnections = new();
    private readonly HashSet<CID> _bots = new();

    private readonly ICustomizationProvider _customizationProvider;
    private readonly IMessageSerializer _messageSerializer;
    private readonly ILogger<ConnectionManager> _logger;

    private readonly RoomManager _roomManager;

    public ConnectionManager(
        ICustomizationProvider customizationProvider,
        IMessageSerializer messageSerializer,
        ILogger<ConnectionManager> logger)
    {
        _customizationProvider = customizationProvider;
        _messageSerializer = messageSerializer;
        _logger = logger;

        _roomManager = new(this);
    }

    private async Task HandleOnConnectAsync(CID cid, WebSocket socket)
    {
        _logger.LogInformation("Connected: {cid}", cid);
        var connection = new PlayerData
        {
            Cid = cid,
            Socket = socket,
            Name = _customizationProvider.AssignDisplayName(),
            Colors = _customizationProvider.AssignTankColors()
        };

        _activeConnections[cid] = connection;
        await SendToSingleAsync(cid, new AssignIdentityServerMessage { Data = connection.ToDto() });
        await _roomManager.HandleOnConnectAsync(cid);
    }

    private async Task HandleOnDisconnectAsync(CID cid)
    {
        _logger.LogInformation("Disconnected: {cid}", cid);
        _activeConnections.Remove(cid);
        await _roomManager.HandleOnDisconnectAsync(cid);
    }

    private async Task RerollClientName(CID cid)
    {
        var con = _activeConnections[cid];
        con.Name = _customizationProvider.AssignDisplayName();
        await SendToSingleAsync(cid, new AssignIdentityServerMessage { Data = con.ToDto() });
    }

    private async Task CustomizeColors(CID cid, TankColors colors)
    {
        var con = _activeConnections[cid];
        con.Colors = colors;
        await SendToSingleAsync(cid, new AssignIdentityServerMessage { Data = con.ToDto() });
    }

    private async Task HandleOnMessageAsync(CID cid, IClientMessage<object?> message)
    {
        _logger.LogDebug("Inbound message from {cid}:\n{message}", cid, message);
        await (message switch
        {
            RerollNameClientMessage when _roomManager.CanPlayerCustomize(cid)
                => RerollClientName(cid),
            CustomizeColorsClientMessage { Data: var dto } when _roomManager.CanPlayerCustomize(cid)
                => CustomizeColors(cid, dto.ToDomain()),
            _ => _roomManager.HandleOnMessageAsync(cid, message)
        });
    }

    public async Task<CID> AddBotAsync()
    {
        var cid = CID.From("CID$" + Guid.NewGuid());
        _bots.Add(cid);
        await _roomManager.HandleOnConnectAsync(cid);
        return cid;
    }

    public async Task SendToSingleAsync<T>(CID cid, IServerMessage<T> message)
    {
        if (!_activeConnections.ContainsKey(cid)) return;
        _logger.LogDebug("Outbound message for {cid}:\n{message}", cid, message);
        var buffer = _messageSerializer.SerializeServerMessage(message);
        await _activeConnections[cid].Socket!.SendAsync(
            new ArraySegment<byte>(buffer),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None);
    }

    public PlayerData DataFor(CID cid)
    {
        if (_bots.Contains(cid))
        {
            return new()
            {
                Cid = cid,
                Socket = null,
                Name = "BOT",
                Colors = _customizationProvider.AssignTankColors(cid.Value.GetHashCode())
            };
        }

        return _activeConnections[cid];
    }

    public async Task AcceptConnectionAsync(CID cid, WebSocket socket, CancellationToken cancellationToken)
    {
        await HandleOnConnectAsync(cid, socket);

        var buffer = new byte[4096];
        try
        {
            while (true)
            {
                Array.Clear(buffer);
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                if (result.CloseStatus.HasValue) break;

                var message = _messageSerializer.DeserializeClientMessage(buffer);
                if (message != null)
                    await HandleOnMessageAsync(cid, message);
            }
        }
        catch (Exception exception)
        {
            _logger.LogWarning("Socket connection ended abruptly.");
            _logger.LogDebug("{exception}", exception);
        }
        finally
        {
            if (socket.State is not (WebSocketState.Closed or WebSocketState.Aborted))
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);

            await HandleOnDisconnectAsync(cid);
        }
    }
}
