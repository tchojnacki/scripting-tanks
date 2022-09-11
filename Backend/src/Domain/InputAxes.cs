namespace Backend.Domain;

public readonly record struct InputAxes
{
    public readonly double Vertical { get; init; }
    public readonly double Horizontal { get; init; }
}
