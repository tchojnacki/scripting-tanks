namespace Backend.Contracts.Data;

public record GameSummaryStateDto : AbstractGameStateDto
{
    public override string Location { get; } = "game-summary";
    public int Remaining { get; init; } = default!;
    public IReadOnlyList<TankDto> Tanks { get; init; } = default!;
    public IReadOnlyList<ScoreboardEntryDto> Scoreboard { get; init; } = default!;
}
