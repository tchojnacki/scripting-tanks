namespace Backend.Contracts.Messages.Client;

public record LeaveLobbyClientMessage : IClientMessage<object?>
{
    public string Tag => "c-leave-lobby";
    public object? Data { get; } = null;
}
