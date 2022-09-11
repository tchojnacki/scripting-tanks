namespace Backend.Contracts.Data;

public record LobbyDto
{
    public string Lid { get; init; } = default!;
    public string Name { get; init; } = default!;
    public int Players { get; init; } = default!;
    public string Location { get; init; } = default!;
}
