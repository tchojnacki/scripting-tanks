using Backend.Contracts.Data;

namespace Backend.Contracts.Messages.Server;

public record NewPlayerServerMessage : IServerMessage<PlayerDto>
{
    public string Tag => "s-new-player";
    public PlayerDto Data { get; init; } = default!;
}
