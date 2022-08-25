using System.Net.WebSockets;

namespace Backend.Services;

public interface IConnectionManager
{
    Task HandleConnectionAsync(WebSocket webSocket, string cid, CancellationToken cancellationToken);
}
