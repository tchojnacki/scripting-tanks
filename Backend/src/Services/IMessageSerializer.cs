using Backend.Contracts.Messages;

namespace Backend.Services;

internal interface IMessageSerializer
{
    bool TryDeserialize(byte[] buffer, out IClientMessage message);
    byte[] Serialize<T>(IServerMessage<T> message);
}
