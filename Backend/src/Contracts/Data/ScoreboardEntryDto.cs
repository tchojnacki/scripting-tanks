namespace Backend.Contracts.Data;

public record ScoreboardEntryDto
{
    public string Cid { get; init; } = default!;
    public string Name { get; init; } = default!;
    public int Score { get; init; } = default!;
}
