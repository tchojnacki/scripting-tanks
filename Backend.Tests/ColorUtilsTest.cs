using Backend.Utils.Common;

namespace Backend.Tests;

public class ColorUtilsTest
{
    [Theory]
    [InlineData(0, 0, 1, 1, 1, 1)]
    [InlineData(0, 0, 0, 0, 0, 0)]
    [InlineData(0, 1, 0.5, 1, 0, 0)]
    [InlineData(300, 1, 0.25, 0.5, 0, 0.5)]
    [InlineData(60, 1, 0.5, 1, 1, 0)]
    public void HslToRgb_ShouldReturnRgbTuple_WhenGivenCorrectHsl(
        double hue, double saturation, double lightness,
        double red, double green, double blue)
    {
        var expected = (red, green, blue);
        var result = ColorUtils.HslToRgb(hue, saturation, lightness);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(-1, 0, 0)]
    [InlineData(361, 0, 0)]
    [InlineData(0, -0.01, 0)]
    [InlineData(0, 2, 0)]
    [InlineData(0, 0, -0.5)]
    [InlineData(0, 0, 3.14)]
    public void HslToRgb_ShouldThrow_WhenGivenInvalidHslValues(double hue, double saturation, double lightness)
    {
        Action act = () => ColorUtils.HslToRgb(hue, saturation, lightness);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
