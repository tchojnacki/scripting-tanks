using System.Net.WebSockets;
using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Client;
using Backend.Contracts.Messages.Server;
using Backend.Domain;
using Backend.Domain.Identifiers;
using Backend.Domain.Rooms;
using Backend.Utils.Mappings;

namespace Backend.Services;

internal sealed class ConnectionManager : IConnectionManager
{
    private readonly Dictionary<Cid, PlayerData> _activeConnections = new();
    private readonly HashSet<Cid> _bots = new();
    private readonly ICustomizationProvider _customizationProvider;
    private readonly ILogger<ConnectionManager> _logger;
    private readonly IMessageSerializer _messageSerializer;
    private readonly IMessageValidator _messageValidator;

    private readonly IRoomManager _roomManager;

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

    public async Task SendToSingleAsync<T>(Cid cid, IServerMessage<T> message)
    {
        if (!_activeConnections.ContainsKey(cid)) return;
        _logger.LogDebug("Outbound message for {Cid}:\n{Message}", cid.ToString(), message);
        var buffer = _messageSerializer.Serialize(message);
        await (_activeConnections[cid].Socket?.SendAsync(
            new(buffer),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None) ?? Task.CompletedTask);
    }

    public PlayerData DataFor(Cid cid)
    {
        if (_bots.Contains(cid))
            return new()
            {
                Cid = cid,
                Socket = null,
                Name = "BOT",
                Colors = _customizationProvider.AssignTankColors(cid.ToString().GetHashCode())
            };

        return _activeConnections[cid];
    }

    public async Task AcceptConnectionAsync(Cid cid, WebSocket socket, CancellationToken cancellationToken)
    {
        await HandleOnConnectAsync(cid, socket);

        var buffer = new byte[4096];
        try
        {
            while (true)
            {
                Array.Clear(buffer);
                var result = await socket.ReceiveAsync(new(buffer), cancellationToken);
                if (result.CloseStatus.HasValue) break;

                if (!_messageSerializer.TryDeserialize(buffer, out var message) ||
                    !_messageValidator.Validate(cid, message)) continue;

                try
                {
                    _logger.LogDebug("Inbound message from {Cid}:\n{Message}", cid.ToString(), message);
                    await HandleOnMessageAsync(cid, message);
                }
                catch (Exception exception)
                {
                    _logger.LogWarning(
                        "Error when processing message:\n{Exception}\n{Message}",
                        exception, message);
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Disconnected socket due to the server closing down");
        }
        catch (Exception exception)
        {
            _logger.LogWarning("Socket connection ended abruptly with error:\n{Exception}", exception);
        }
        finally
        {
            if (socket.State is not (WebSocketState.Closed or WebSocketState.Aborted))
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);

            await HandleOnDisconnectAsync(cid);
        }
    }

    public async Task HandleOnConnectAsync(Cid cid, WebSocket socket)
    {
        _logger.LogInformation("Connected: {Cid}", cid.ToString());
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

    private async Task HandleOnDisconnectAsync(Cid cid)
    {
        _logger.LogInformation("Disconnected: {Cid}", cid.ToString());
        _activeConnections.Remove(cid);
        await _roomManager.HandleOnDisconnectAsync(cid);
    }

    private async Task RerollClientName(Cid cid)
    {
        var con = _activeConnections[cid];
        con.Name = _customizationProvider.AssignDisplayName();
        await SendToSingleAsync(cid, new AssignIdentityServerMessage { Data = con.ToDto() });
    }

    private async Task CustomizeColors(Cid cid, TankColors colors)
    {
        var con = _activeConnections[cid];
        con.Colors = colors;
        await SendToSingleAsync(cid, new AssignIdentityServerMessage { Data = con.ToDto() });
    }

    private Task HandleOnMessageAsync(Cid cid, IClientMessage message) => message switch
    {
        RerollNameClientMessage => RerollClientName(cid),
        CustomizeColorsClientMessage { Data: var dto } => CustomizeColors(cid, dto.ToDomain()),
        AddBotClientMessage => AddBotAsync(((GameRoom)_roomManager.RoomContaining(cid)).Lid),
        _ => _roomManager.HandleOnMessageAsync(cid, message)
    };

    private async Task AddBotAsync(Lid lid)
    {
        var cid = Cid.GenerateUnique();
        _bots.Add(cid);
        await _roomManager.HandleOnConnectAsync(cid);
        await _roomManager.JoinGameRoomAsync(cid, lid);
    }
}