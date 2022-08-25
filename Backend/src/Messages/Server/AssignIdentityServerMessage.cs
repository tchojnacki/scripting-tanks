using Backend.Dtos;

namespace Backend.Messages.Server;

public readonly record struct AssignIdentityServerMessage : IServerMessage<PlayerDto>
{
    public string Tag => "s-assign-identity";
    public PlayerDto Data { get; init; }
}
