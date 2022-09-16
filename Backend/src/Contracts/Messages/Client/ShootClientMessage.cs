namespace Backend.Contracts.Messages.Client;

public sealed record ShootClientMessage : IClientMessage
{
    public string Tag => "c-shoot";
}
