using System.Net.WebSockets;
using System.Text;
using Backend.Utils;

namespace Backend.Services;

public class ConnectionManager : IConnectionManager
{
    private readonly Dictionary<string, ConnectionData> _activeConnections = new();

    private readonly INameProvider _nameProvider;

    public ConnectionManager(INameProvider nameProvider) => _nameProvider = nameProvider;

    private async Task HandleOnConnectAsync(WebSocket webSocket, string cid)
    {
        var connection = new ConnectionData
        {
            Socket = webSocket,
            DisplayName = _nameProvider.GenerateRandomName()
        };

        _activeConnections[cid] = connection;
        await SendToSingleAsync(cid, $@"{{""cid"": ""{cid}""}}");
    }

    private Task HandleOnDisconnectAsync(string cid)
    {
        _activeConnections.Remove(cid);
        return Task.CompletedTask;
    }

    private Task HandleMessageAsync(string cid, string content)
    {
        Console.WriteLine(content);
        return Task.CompletedTask;
    }

    public async Task SendToSingleAsync(string cid, string message)
    {
        await _activeConnections[cid].Socket.SendAsync(
            new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None);
    }

    public async Task HandleConnectionAsync(WebSocket webSocket, string cid, CancellationToken cancellationToken)
    {
        await HandleOnConnectAsync(webSocket, cid);

        var buffer = new byte[4096];
        WebSocketReceiveResult result;

        try
        {
            do
            {
                Array.Clear(buffer);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
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
            if (webSocket.State is not (WebSocketState.Closed or WebSocketState.Aborted))
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);
            }
            await HandleOnDisconnectAsync(cid);
        }
    }
}
