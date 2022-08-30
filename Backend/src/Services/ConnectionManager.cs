using System.Net.WebSockets;
using Backend.Domain;
using Backend.Identifiers;
using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Server;
using Backend.Contracts.Messages.Client;
using Backend.Utils.Mappers;

namespace Backend.Services;

public class ConnectionManager : IConnectionManager
{
    private readonly Dictionary<CID, ConnectionData> _activeConnections = new();

    private readonly ICustomizationProvider _customizationProvider;
    private readonly IMessageSerializer _messageSerializer;

    public ConnectionManager(ICustomizationProvider customizationProvider, IMessageSerializer messageSerializer)
    {
        _customizationProvider = customizationProvider;
        _messageSerializer = messageSerializer;
    }

    private async Task HandleOnConnectAsync(CID cid, WebSocket socket)
    {
        var connection = new ConnectionData
        {
            Socket = socket,
            DisplayName = _customizationProvider.AssignDisplayName(),
            Colors = _customizationProvider.AssignTankColors()
        };

        _activeConnections[cid] = connection;
        await SendToSingleAsync(cid, new AssignIdentityServerMessage { Data = connection.ToDto(cid) });
    }

    private Task HandleOnDisconnectAsync(CID cid)
    {
        _activeConnections.Remove(cid);
        return Task.CompletedTask;
    }

    private async Task ReceiveMessageAsync(CID cid, IClientMessage<object?> message)
    {
        switch (message)
        {
            case RerollNameClientMessage:
                _activeConnections[cid].DisplayName = _customizationProvider.AssignDisplayName();
                await SendToSingleAsync(cid, new AssignIdentityServerMessage
                {
                    Data = _activeConnections[cid].ToDto(cid)
                });
                break;
            case CustomizeColorsClientMessage { Data: var dto }:
                _activeConnections[cid].Colors = dto.Colors.ToDomain();
                await SendToSingleAsync(cid, new AssignIdentityServerMessage
                {
                    Data = _activeConnections[cid].ToDto(cid)
                });
                break;
            default:
                break;
        }
    }

    public async Task SendToSingleAsync<T>(CID cid, IServerMessage<T> message)
    {
        var buffer = _messageSerializer.SerializeServerMessage(message);

        await _activeConnections[cid].Socket.SendAsync(
            new ArraySegment<byte>(buffer),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None);
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
                    await ReceiveMessageAsync(cid, message);
            }
        }
        catch
        {
            Console.WriteLine("Socket connection ended abruptly.");
        }
        finally
        {
            if (socket.State is not (WebSocketState.Closed or WebSocketState.Aborted))
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);

            await HandleOnDisconnectAsync(cid);
        }
    }
}
