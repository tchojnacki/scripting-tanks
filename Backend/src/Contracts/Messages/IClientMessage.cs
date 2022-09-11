namespace Backend.Contracts.Messages;

public interface IClientMessage
{
    string Tag { get; }
}

public interface IClientMessage<out T> : IClientMessage
{
    T Data { get; }
}
