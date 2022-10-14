namespace Backend.Contracts.Data;

public sealed record PlayerDto
{
    public string Cid { get; init; } = default!;
    public string Name { get; init; } = default!;
    public TankColorsDto Colors { get; init; } = default!;
    public bool Bot { get; init; } = default!;
}
