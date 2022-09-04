namespace Backend.Contracts.Data;

public record GameWaitingStateDto : AbstractRoomStateDto
{
    public override string Location { get; } = "game-waiting";
    public string Name { get; init; } = default!;
    public string Owner { get; init; } = default!;
    public IReadOnlyList<PlayerDto> Players { get; init; } = default!;
}
