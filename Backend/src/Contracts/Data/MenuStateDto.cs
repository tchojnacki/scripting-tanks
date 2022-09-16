namespace Backend.Contracts.Data;

public sealed record MenuStateDto : AbstractRoomStateDto
{
    public override string Location { get; } = "menu";
    public IReadOnlyList<LobbyDto> Lobbies { get; init; } = default!;
}
