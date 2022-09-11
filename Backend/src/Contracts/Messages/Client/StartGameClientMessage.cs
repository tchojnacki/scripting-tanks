namespace Backend.Contracts.Messages.Client;

public record StartGameClientMessage : IClientMessage
{
    public string Tag => "c-start-game";
}
