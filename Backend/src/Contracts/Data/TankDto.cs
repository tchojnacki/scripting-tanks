namespace Backend.Contracts.Data;

public sealed record TankDto : AbstractEntityDto
{
    public override string Kind { get; } = "tank";
    public string CID { get; init; } = default!;
    public string Name { get; init; } = default!;
    public TankColorsDto Colors { get; init; } = default!;
    public double Pitch { get; init; } = default!;
    public double Barrel { get; init; } = default!;
}
