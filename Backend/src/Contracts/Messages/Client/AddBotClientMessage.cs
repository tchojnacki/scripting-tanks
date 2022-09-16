namespace Backend.Contracts.Messages.Client;

public sealed record AddBotClientMessage : IClientMessage
{
    public string Tag => "c-add-bot";
}
