namespace Backend.Contracts.Messages.Client;

public record EnterLobbyClientMessage : IClientMessage<string>
{
    public string Tag => "c-enter-lobby";
    public string Data { get; init; } = default!;
}
