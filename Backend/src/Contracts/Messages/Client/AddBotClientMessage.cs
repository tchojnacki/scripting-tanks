namespace Backend.Contracts.Messages.Client;

public record AddBotClientMessage : IClientMessage<object?>
{
    public string Tag => "c-add-bot";
    public object? Data { get; } = null;
}
