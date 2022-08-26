namespace Backend.Contracts.Data;

public record BulletDto : AbstractEntityDto
{
    public override string Kind { get; } = "bullet";
    public string Owner { get; init; } = default!;
    public double Radius { get; init; } = default!;
}
