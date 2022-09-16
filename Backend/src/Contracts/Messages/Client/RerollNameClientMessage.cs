namespace Backend.Contracts.Messages.Client;

public sealed record RerollNameClientMessage : IClientMessage
{
    public string Tag => "c-reroll-name";
}
