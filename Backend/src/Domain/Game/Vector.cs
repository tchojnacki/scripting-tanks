namespace Backend.Domain.Game;

public readonly record struct Vector
{
    public readonly double X { get; }
    public readonly double Y { get; }
    public readonly double Z { get; }

    public Vector(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}
