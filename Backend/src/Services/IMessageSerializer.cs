using Backend.Contracts.Messages;

namespace Backend.Services;

public interface IMessageSerializer
{
    IClientMessage? DeserializeClientMessage(byte[] buffer);
    byte[] SerializeServerMessage<T>(IServerMessage<T> message);
}
