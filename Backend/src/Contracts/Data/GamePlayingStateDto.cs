namespace Backend.Contracts.Data;

public record GamePlayingStateDto : AbstractGameStateDto
{
    public override string Location { get; } = "game-playing";
    public double Radius { get; init; } = default!;
    public List<AbstractEntityDto> Entities { get; init; } = default!;
    public List<ScoreboardEntryDto> Scoreboard { get; init; } = default!;
}
