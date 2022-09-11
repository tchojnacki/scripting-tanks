namespace Backend.Contracts.Messages.Client;

public record RerollNameClientMessage : IClientMessage
{
    public string Tag => "c-reroll-name";
}
