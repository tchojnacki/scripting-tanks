namespace Backend.Contracts.Data;

public sealed record LobbyDto
{
    public string Lid { get; init; } = default!;
    public string Name { get; init; } = default!;
    public int Players { get; init; }
    public string Location { get; init; } = default!;
}
