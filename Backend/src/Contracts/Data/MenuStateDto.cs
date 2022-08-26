namespace Backend.Contracts.Data;

public record MenuStateDto : AbstractRoomStateDto
{
    public override string Location { get; } = "menu";
    public List<LobbyDto> Lobbies { get; init; } = default!;
}
