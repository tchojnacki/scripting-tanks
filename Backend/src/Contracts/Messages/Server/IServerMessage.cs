namespace Backend.Contracts.Messages.Server;

public interface IServerMessage<out T>
{
    string Tag { get; }
    T Data { get; }
}
