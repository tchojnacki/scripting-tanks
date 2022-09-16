using Backend.Domain.Identifiers;
using Backend.Contracts.Messages;

namespace Backend.Services;

internal interface IBroadcastHelper
{
    public Task BroadcastToRoom<T>(LID lid, IServerMessage<T> message);
    public Task BroadcastToMenu<T>(IServerMessage<T> message);
}
