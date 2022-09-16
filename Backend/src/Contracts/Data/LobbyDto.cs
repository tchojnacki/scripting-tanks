namespace Backend.Contracts.Data;

public sealed record LobbyDto
{
    public string LID { get; init; } = default!;
    public string Name { get; init; } = default!;
    public int Players { get; init; } = default!;
    public string Location { get; init; } = default!;
}
