namespace Backend.Contracts.Messages;

public interface IClientMessage<out T>
{
    string Tag { get; }
    T Data { get; }
}
