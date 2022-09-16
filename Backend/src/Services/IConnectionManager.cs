using System.Net.WebSockets;
using Backend.Domain;
using Backend.Domain.Identifiers;
using Backend.Contracts.Messages;

namespace Backend.Services;

internal interface IConnectionManager
{
    Task AcceptConnectionAsync(CID cid, WebSocket socket, CancellationToken cancellationToken);
    Task SendToSingleAsync<T>(CID cid, IServerMessage<T> message);
    PlayerData DataFor(CID cid);
}
