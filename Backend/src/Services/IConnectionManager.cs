using System.Net.WebSockets;
using Backend.Contracts.Messages;
using Backend.Domain;
using Backend.Domain.Identifiers;

namespace Backend.Services;

internal interface IConnectionManager
{
    Task AcceptConnectionAsync(Cid cid, WebSocket socket, CancellationToken cancellationToken);
    Task SendToSingleAsync<T>(Cid cid, IServerMessage<T> message);
    PlayerData DataFor(Cid cid);
}