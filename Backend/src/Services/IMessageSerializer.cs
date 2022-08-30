using Backend.Contracts.Messages;

namespace Backend.Services;

public interface IMessageSerializer
{
    IClientMessage<object?>? DeserializeClientMessage(byte[] buffer);
    byte[] SerializeServerMessage<T>(IServerMessage<T> message);
}
