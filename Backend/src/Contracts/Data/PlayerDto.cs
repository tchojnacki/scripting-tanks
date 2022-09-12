namespace Backend.Contracts.Data;

public record PlayerDto
{
    public string CID { get; init; } = default!;
    public string Name { get; init; } = default!;
    public TankColorsDto Colors { get; init; } = default!;
    public bool Bot { get; init; } = default!;
}
