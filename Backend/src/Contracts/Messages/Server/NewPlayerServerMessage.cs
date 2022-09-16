using Backend.Contracts.Data;

namespace Backend.Contracts.Messages.Server;

public sealed record NewPlayerServerMessage : IServerMessage<PlayerDto>
{
    public string Tag => "s-new-player";
    public PlayerDto Data { get; init; } = default!;
}
