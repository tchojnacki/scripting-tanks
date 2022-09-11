namespace Backend.Contracts.Messages.Server;

public record PlayerLeftServerMessage : IServerMessage<string>
{
    public string Tag => "s-player-left";
    public string Data { get; init; } = default!;
}
