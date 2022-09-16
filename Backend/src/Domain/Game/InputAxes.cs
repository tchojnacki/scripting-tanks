namespace Backend.Domain.Game;

public readonly record struct InputAxes
{
    public readonly double Vertical { get; init; }
    public readonly double Horizontal { get; init; }
}
