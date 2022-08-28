using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Backend.Domain;
using Backend.Domain.Identifiers;
using Backend.Contracts.Messages.Client;
using Backend.Contracts.Messages.Server;
using Backend.Utils.Mappers;

namespace Backend.Services;

public class ConnectionManager : IConnectionManager
{
    private readonly Dictionary<CID, ConnectionData> _activeConnections = new();

    private async Task HandleOnConnectAsync(CID cid, WebSocket socket)
    {
        var connection = new ConnectionData
        {
            Socket = socket,
            DisplayName = NameProvider.GenerateRandomName(),
            Colors = new() { TankColor = "#000000", TurretColor = "#000000" }
        };

        _activeConnections[cid] = connection;
        await SendToSingleAsync(cid, new AssignIdentityServerMessage { Data = connection.ToDto(cid) });
    }

    private Task HandleOnDisconnectAsync(CID cid)
    {
        _activeConnections.Remove(cid);
        return Task.CompletedTask;
    }

    private async Task HandleMessageAsync(CID cid, string content)
    {
        var tag = JsonDocument.Parse(content).RootElement.GetProperty("tag").GetString()!;
        var type = ClientMessageFactory.TagToType(tag)!;
        var message = JsonSerializer.Deserialize(content, type, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        switch (message)
        {
            case RerollNameClientMessage:
                _activeConnections[cid].DisplayName = NameProvider.GenerateRandomName();
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
        var buffer = JsonSerializer.SerializeToUtf8Bytes(message, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await _activeConnections[cid].Socket.SendAsync(
            new ArraySegment<byte>(buffer),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None);
    }

    public async Task HandleConnectionAsync(CID cid, WebSocket socket, CancellationToken cancellationToken)
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

                await HandleMessageAsync(cid, Encoding.UTF8.GetString(buffer).TrimEnd('\0'));
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
