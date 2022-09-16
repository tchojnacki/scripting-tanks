namespace Backend.Contracts.Messages.Client;

public sealed record LeaveLobbyClientMessage : IClientMessage
{
    public string Tag => "c-leave-lobby";
}
