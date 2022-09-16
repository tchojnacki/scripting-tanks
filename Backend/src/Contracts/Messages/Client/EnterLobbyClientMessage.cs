namespace Backend.Contracts.Messages.Client;

public sealed record EnterLobbyClientMessage : IClientMessage<string>
{
    public string Tag => "c-enter-lobby";
    public string Data { get; init; } = default!;
}
