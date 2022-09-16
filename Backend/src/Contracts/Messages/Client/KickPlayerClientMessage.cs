namespace Backend.Contracts.Messages.Client;

public sealed record KickPlayerClientMessage : IClientMessage<string>
{
    public string Tag => "c-kick-player";
    public string Data { get; init; } = default!;
}
