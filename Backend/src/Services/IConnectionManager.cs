using System.Net.WebSockets;
using Backend.Utils.Identifiers;

namespace Backend.Services;

public interface IConnectionManager
{
    Task HandleConnectionAsync(CID cid, WebSocket socket, CancellationToken cancellationToken);
}
