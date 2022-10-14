using Backend.Contracts.Messages;
using Backend.Domain.Identifiers;

namespace Backend.Services;

internal interface IBroadcastHelper
{
    public Task BroadcastToRoom<T>(Lid lid, IServerMessage<T> message);
    public Task BroadcastToMenu<T>(IServerMessage<T> message);
}