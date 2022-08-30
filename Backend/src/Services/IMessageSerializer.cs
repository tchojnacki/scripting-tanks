using Backend.Contracts.Messages.Client;
using Backend.Contracts.Messages.Server;

namespace Backend.Services;

public interface IMessageSerializer
{
    IClientMessage<object?>? DeserializeClientMessage(byte[] buffer);
    byte[] SerializeServerMessage<T>(IServerMessage<T> message);
}
