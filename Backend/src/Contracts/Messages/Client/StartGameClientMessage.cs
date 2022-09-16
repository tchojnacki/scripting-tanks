namespace Backend.Contracts.Messages.Client;

public sealed record StartGameClientMessage : IClientMessage
{
    public string Tag => "c-start-game";
}
