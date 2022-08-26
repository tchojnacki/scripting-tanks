namespace Backend.Contracts.Messages.Client;

public record RerollNameClientMessage : IClientMessage<object?>
{
    public string Tag => "c-reroll-name";
    public object? Data { get; } = null;
}
