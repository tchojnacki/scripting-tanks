namespace Backend.Contracts.Data;

public record InputAxesDto
{
    public double Vertical { get; init; } = default!;
    public double Horizontal { get; init; } = default!;
}
