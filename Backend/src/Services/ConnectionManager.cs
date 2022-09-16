using System.Net.WebSockets;
using Backend.Domain;
using Backend.Domain.Rooms;
using Backend.Domain.Identifiers;
using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Server;
using Backend.Contracts.Messages.Client;
using Backend.Utils.Mappings;

namespace Backend.Services;

internal sealed class ConnectionManager : IConnectionManager
{
    private readonly Dictionary<CID, PlayerData> _activeConnections = new();
    private readonly HashSet<CID> _bots = new();

    private readonly IRoomManager _roomManager;
    private readonly ICustomizationProvider _customizationProvider;
    private readonly IMessageSerializer _messageSerializer;
    private readonly IMessageValidator _messageValidator;
    private readonly ILogger<ConnectionManager> _logger;

    public ConnectionManager(
        IRoomManager roomManager,
        ICustomizationProvider customizationProvider,
        IMessageSerializer messageSerializer,
        IMessageValidator messageValidator,
        ILogger<ConnectionManager> logger)
    {
        _roomManager = roomManager;
        _customizationProvider = customizationProvider;
        _messageSerializer = messageSerializer;
        _messageValidator = messageValidator;
        _logger = logger;
    }

    private async Task HandleOnConnectAsync(CID cid, WebSocket socket)
    {
        _logger.LogInformation("Connected: {cid}", cid);
        var connection = new PlayerData
        {
            CID = cid,
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

    private Task HandleOnMessageAsync(CID cid, IClientMessage message) => message switch
    {
        RerollNameClientMessage => RerollClientName(cid),
        CustomizeColorsClientMessage { Data: var dto } => CustomizeColors(cid, dto.ToDomain()),
        AddBotClientMessage => AddBotAsync(((GameRoom)_roomManager.RoomContaining(cid)).LID),
        _ => _roomManager.HandleOnMessageAsync(cid, message)
    };

    private async Task AddBotAsync(LID lid)
    {
        var cid = CID.GenerateUnique();
        _bots.Add(cid);
        await _roomManager.HandleOnConnectAsync(cid);
        await _roomManager.JoinGameRoomAsync(cid, lid);
    }

    public async Task SendToSingleAsync<T>(CID cid, IServerMessage<T> message)
    {
        if (!_activeConnections.ContainsKey(cid)) return;
        _logger.LogDebug("Outbound message for {cid}:\n{message}", cid, message);
        var buffer = _messageSerializer.Serialize(message);
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
                CID = cid,
                Socket = null,
                Name = "BOT",
                Colors = _customizationProvider.AssignTankColors(cid.ToString().GetHashCode())
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

                if (_messageSerializer.TryDeserialize(buffer, out var message) &&
                    _messageValidator.Validate(cid, message))
                {
                    try
                    {
                        _logger.LogDebug("Inbound message from {cid}:\n{message}", cid, message);
                        await HandleOnMessageAsync(cid, message);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogWarning("Error when processing message:\n{message}\n{exception}", message, exception);
                    }
                }
            }
        }
        catch (Exception exception)
        {
            _logger.LogWarning("Socket connection ended abruptly with error:\n{exception}", exception);
        }
        finally
        {
            if (socket.State is not (WebSocketState.Closed or WebSocketState.Aborted))
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);

            await HandleOnDisconnectAsync(cid);
        }
    }
}
