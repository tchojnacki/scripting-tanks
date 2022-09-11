namespace Backend.Contracts.Messages.Client;

public record CloseLobbyClientMessage : IClientMessage
{
    public string Tag => "c-close-lobby";
}
