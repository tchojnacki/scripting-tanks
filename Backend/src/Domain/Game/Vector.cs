using static System.Math;

namespace Backend.Domain.Game;

internal readonly record struct Vector
{
    public Vector(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public readonly double X { get; }
    public readonly double Y { get; }
    public readonly double Z { get; }

    public static Vector UnitWithPitch(double pitch) => new(Sin(pitch), 0, Cos(pitch));

    public double Length => Sqrt(X * X + Y * Y + Z * Z);

    public Vector Normalized => this / Length;

    public double Pitch => Atan2(X, Z);

    public static double Dot(Vector v, Vector w) => v.X * w.X + v.Y * w.Y + v.Z * w.Z;

    public static Vector Cross(Vector v, Vector w)
        => new(v.Y * w.Z - v.Z * w.Y, v.Z * w.X - v.X * w.Z, v.X * w.Y - v.Y * w.X);

    public static Vector operator +(Vector v, Vector w) => new(v.X + w.X, v.Y + w.Y, v.Z + w.Z);

    public static Vector operator *(double factor, Vector v) => new(v.X * factor, v.Y * factor, v.Z * factor);

    public static Vector operator -(Vector v) => (-1) * v;

    public static Vector operator -(Vector v, Vector w) => v + (-w);

    public static Vector operator *(Vector v, double factor) => factor * v;

    public static Vector operator /(Vector v, double divisor) => 1.0 / divisor * v;

    public override string ToString() => $"({X}, {Y}, {Z})";
}
