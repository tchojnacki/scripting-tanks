namespace Backend.Contracts.Messages.Client;

public record CreateLobbyClientMessage : IClientMessage
{
    public string Tag => "c-create-lobby";
}
