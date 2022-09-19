using static System.Math;

namespace Backend.Utils.Common;

internal static class MathUtils
{
    public static double AngleDiff(double from, double to) => Atan2(Sin(to - from), Cos(to - from));

    public static double AbsAngleDiff(double alpha, double beta) => Abs(AngleDiff(alpha, beta));
}
