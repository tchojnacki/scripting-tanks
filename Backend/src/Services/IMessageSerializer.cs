using Backend.Contracts.Messages;

namespace Backend.Services;

public interface IMessageSerializer
{
    bool TryDeserialize(byte[] buffer, out IClientMessage message);
    byte[] Serialize<T>(IServerMessage<T> message);
}
