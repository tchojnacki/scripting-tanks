namespace Backend.Contracts.Messages.Server;

public interface IServerMessage<T>
{
    string Tag { get; }
    T Data { get; }
}
