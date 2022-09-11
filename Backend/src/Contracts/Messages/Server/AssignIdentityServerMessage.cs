using Backend.Contracts.Data;

namespace Backend.Contracts.Messages.Server;

public record AssignIdentityServerMessage : IServerMessage<PlayerDto>
{
    public string Tag => "s-assign-identity";
    public PlayerDto Data { get; init; } = default!;
}
