namespace Backend.Domain;

public record TankColors
{
    public string TankColor { get; init; } = default!;
    public string TurretColor { get; init; } = default!;
}
