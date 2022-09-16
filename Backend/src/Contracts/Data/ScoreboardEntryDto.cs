namespace Backend.Contracts.Data;

public sealed record ScoreboardEntryDto
{
    public string CID { get; init; } = default!;
    public string Name { get; init; } = default!;
    public int Score { get; init; } = default!;
}
