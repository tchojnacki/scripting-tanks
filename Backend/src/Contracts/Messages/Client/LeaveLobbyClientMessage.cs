namespace Backend.Contracts.Messages.Client;

public record LeaveLobbyClientMessage : IClientMessage
{
    public string Tag => "c-leave-lobby";
}
