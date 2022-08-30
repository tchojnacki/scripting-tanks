namespace Backend.Contracts.Messages.Client;

public interface IClientMessage<out T>
{
    string Tag { get; }
    T Data { get; }
}
