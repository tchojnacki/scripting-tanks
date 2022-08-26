using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Backend.Utils;
using Backend.Utils.Identifiers;
using Backend.Contracts.Messages.Server;

namespace Backend.Services;

public class ConnectionManager : IConnectionManager
{
    private readonly Dictionary<CID, ConnectionData> _activeConnections = new();

    private async Task HandleOnConnectAsync(CID cid, WebSocket socket)
    {
        var connection = new ConnectionData
        {
            Socket = socket,
            DisplayName = NameProvider.GenerateRandomName()
        };

        _activeConnections[cid] = connection;
        await SendToSingleAsync(cid, new AssignIdentityServerMessage
        {
            Data = new()
            {
                Cid = cid.Value,
                Name = connection.DisplayName,
                Colors = new() { "#ff0000", "#00ff00" },
                Bot = false,
            }
        });
    }

    private Task HandleOnDisconnectAsync(CID cid)
    {
        _activeConnections.Remove(cid);
        return Task.CompletedTask;
    }

    private Task HandleMessageAsync(CID cid, string content)
    {
        Console.WriteLine(content);
        return Task.CompletedTask;
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
        WebSocketReceiveResult result;

        try
        {
            do
            {
                Array.Clear(buffer);
                result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                await HandleMessageAsync(cid, Encoding.UTF8.GetString(buffer));
            }
            while (!result.CloseStatus.HasValue);
        }
        catch
        {
            Console.WriteLine("Socket connection ended abruptly.");
        }
        finally
        {
            if (socket.State is not (WebSocketState.Closed or WebSocketState.Aborted))
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);
            }
            await HandleOnDisconnectAsync(cid);
        }
    }
}
