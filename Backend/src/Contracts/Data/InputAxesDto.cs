namespace Backend.Contracts.Data;

public sealed record InputAxesDto
{
    public double Vertical { get; init; }
    public double Horizontal { get; init; }
}
