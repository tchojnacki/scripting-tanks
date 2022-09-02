using System.Net.WebSockets;
using Backend.Identifiers;
using Backend.Domain;
using Backend.Contracts.Messages;

namespace Backend.Services;

public interface IConnectionManager
{
    Task AcceptConnectionAsync(CID cid, WebSocket socket, CancellationToken cancellationToken);
    Task SendToSingleAsync<T>(CID cid, IServerMessage<T> message);
    Task<CID> AddBotAsync();
    ConnectionData PlayerData(CID cid);
}
