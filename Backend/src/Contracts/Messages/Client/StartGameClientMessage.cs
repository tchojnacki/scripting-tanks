namespace Backend.Contracts.Messages.Client;

public record StartGameClientMessage : IClientMessage<object?>
{
    public string Tag => "c-start-game";
    public object? Data { get; init; } = null;
}
