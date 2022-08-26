namespace Backend.Contracts.Messages.Client;

public record CreateLobbyClientMessage : IClientMessage<object?>
{
    public string Tag => "c-create-lobby";
    public object? Data { get; } = null;
}
