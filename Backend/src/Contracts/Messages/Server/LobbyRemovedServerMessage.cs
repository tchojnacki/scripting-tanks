namespace Backend.Contracts.Messages.Server;

public sealed record LobbyRemovedServerMessage : IServerMessage<string>
{
    public string Tag => "s-lobby-removed";
    public string Data { get; init; } = default!;
}
