namespace Backend.Contracts.Messages;

public interface IServerMessage<out T>
{
    string Tag { get; }
    T Data { get; }
}
