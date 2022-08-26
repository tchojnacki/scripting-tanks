namespace Backend.Contracts.Messages.Client;

public record CloseLobbyClientMessage : IClientMessage<object?>
{
    public string Tag => "c-close-lobby";
    public object? Data { get; } = null;
}
