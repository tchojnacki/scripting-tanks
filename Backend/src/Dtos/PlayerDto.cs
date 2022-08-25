namespace Backend.Dtos;

public readonly record struct PlayerDto
{
    public readonly string Cid { get; init; }
    public readonly string Name { get; init; }
    public readonly List<string> Colors { get; init; }
    public readonly bool Bot { get; init; }
}
