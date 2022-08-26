namespace Backend.Contracts.Data;

public record TankDto : AbstractEntityDto
{
    public override string Kind { get; } = "tank";
    public string Cid { get; init; } = default!;
    public string Name { get; init; } = default!;
    public List<string> Colors { get; init; } = default!;
    public double Pitch { get; init; } = default!;
    public double Barrel { get; init; } = default!;
}
