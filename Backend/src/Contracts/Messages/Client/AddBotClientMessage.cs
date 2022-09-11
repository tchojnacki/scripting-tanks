namespace Backend.Contracts.Messages.Client;

public record AddBotClientMessage : IClientMessage
{
    public string Tag => "c-add-bot";
}
