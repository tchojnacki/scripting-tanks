namespace Backend.Contracts.Data;

public record ColorCustomizationDto
{
    public TankColorsDto Colors { get; init; } = default!;
}
