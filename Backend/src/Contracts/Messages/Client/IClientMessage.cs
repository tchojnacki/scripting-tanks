namespace Backend.Contracts.Messages.Client;

public interface IClientMessage<T>
{
    string Tag { get; }
    T Data { get; }
}
