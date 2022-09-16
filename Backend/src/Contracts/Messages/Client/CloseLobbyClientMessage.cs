namespace Backend.Contracts.Messages.Client;

public sealed record CloseLobbyClientMessage : IClientMessage
{
    public string Tag => "c-close-lobby";
}
