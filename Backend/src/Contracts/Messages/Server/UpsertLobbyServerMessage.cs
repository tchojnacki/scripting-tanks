using Backend.Contracts.Data;

namespace Backend.Contracts.Messages.Server;

public record UpsertLobbyServerMessage : IServerMessage<LobbyDto>
{
    public string Tag => "s-upsert-lobby";
    public LobbyDto Data { get; init; } = default!;
}
