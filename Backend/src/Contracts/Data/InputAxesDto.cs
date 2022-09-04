namespace Backend.Contracts.Data;

public readonly record struct InputAxesDto
{
    public readonly double Vertical { get; init; }
    public readonly double Horizontal { get; init; }
}
