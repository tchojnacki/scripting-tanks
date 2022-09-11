namespace Backend.Contracts.Messages.Client;

public record ShootClientMessage : IClientMessage
{
    public string Tag => "c-shoot";
}
