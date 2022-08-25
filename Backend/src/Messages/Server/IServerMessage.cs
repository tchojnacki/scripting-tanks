namespace Backend.Messages.Server;

public interface IServerMessage<T>
{
    string Tag { get; }
    T Data { get; init; }
}
