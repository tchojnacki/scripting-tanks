namespace Backend.Contracts.Messages.Client;

public record ShootClientMessage : IClientMessage<object?>
{
    public string Tag => "c-shoot";
    public object? Data { get; } = null;
}
