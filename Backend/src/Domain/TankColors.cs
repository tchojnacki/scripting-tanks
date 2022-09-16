namespace Backend.Domain;

internal sealed record TankColors
{
    public string TankColor { get; init; } = default!;
    public string TurretColor { get; init; } = default!;
}
