namespace Backend.Utils;

public static class ColorUtils
{
    public static (double red, double green, double blue) HslToRgb(double hue, double saturation, double lightness)
    {
        if (hue is not (>= 0 and < 360))
            throw new ArgumentOutOfRangeException(nameof(hue));

        if (saturation is not (>= 0 and <= 1))
            throw new ArgumentOutOfRangeException(nameof(saturation));

        if (lightness is not (>= 0 and <= 1))
            throw new ArgumentOutOfRangeException(nameof(lightness));

        var C = (1 - Math.Abs(2 * lightness - 1)) * saturation;
        var huePrim = hue / 60;
        var X = C * (1 - Math.Abs(huePrim % 2 - 1));

        var (rOne, gOne, bOne) = huePrim switch
        {
            >= 0 and < 1 => (C, X, 0.0),
            >= 1 and < 2 => (X, C, 0.0),
            >= 2 and < 3 => (0.0, C, X),
            >= 3 and < 4 => (0.0, X, C),
            >= 4 and < 5 => (X, 0.0, C),
            >= 5 and < 6 => (C, 0.0, X),
            _ => throw new InvalidOperationException()
        };

        var m = lightness - C / 2;
        return (rOne + m, gOne + m, bOne + m);
    }

    public static string RgbToString(double red, double green, double blue)
    {
        if (red is not (>= 0 and <= 1))
            throw new ArgumentOutOfRangeException(nameof(red));

        if (green is not (>= 0 and <= 1))
            throw new ArgumentOutOfRangeException(nameof(green));

        if (blue is not (>= 0 and <= 1))
            throw new ArgumentOutOfRangeException(nameof(blue));

        return $"#{(int)(red * 255):X2}{(int)(green * 255):X2}{(int)(blue * 255):X2}".ToLowerInvariant();
    }
}
