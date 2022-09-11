namespace Backend.Contracts.Data;

public record MenuStateDto : AbstractRoomStateDto
{
    public override string Location { get; } = "menu";
    public IReadOnlyList<LobbyDto> Lobbies { get; init; } = default!;
}
