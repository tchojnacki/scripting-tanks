namespace Backend.Utils.Common;

internal static class ColorUtils
{
    public static (double red, double green, double blue) HslToRgb(double hue, double saturation, double lightness)
    {
        if (hue is not (>= 0 and < 360))
            throw new ArgumentOutOfRangeException(nameof(hue));

        if (saturation is not (>= 0 and <= 1))
            throw new ArgumentOutOfRangeException(nameof(saturation));

        if (lightness is not (>= 0 and <= 1))
            throw new ArgumentOutOfRangeException(nameof(lightness));

        var c = (1 - Math.Abs(2 * lightness - 1)) * saturation;
        var huePrim = hue / 60;
        var x = c * (1 - Math.Abs(huePrim % 2 - 1));

        var (rOne, gOne, bOne) = huePrim switch
        {
            >= 0 and < 1 => (c, x, 0.0),
            >= 1 and < 2 => (x, c, 0.0),
            >= 2 and < 3 => (0.0, c, x),
            >= 3 and < 4 => (0.0, x, c),
            >= 4 and < 5 => (x, 0.0, c),
            >= 5 and < 6 => (c, 0.0, x),
            _ => throw new InvalidOperationException()
        };

        var m = lightness - c / 2;
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
