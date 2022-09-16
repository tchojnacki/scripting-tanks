namespace Backend.Contracts.Messages.Client;

public sealed record CreateLobbyClientMessage : IClientMessage
{
    public string Tag => "c-create-lobby";
}
