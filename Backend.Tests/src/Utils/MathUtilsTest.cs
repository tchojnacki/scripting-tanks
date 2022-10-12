using Backend.Utils.Common;

using static System.Math;

namespace Backend.Tests.Utils;

public class MathUtilsTest
{
    [Theory]
    [InlineData(1.5, 1.5, 0)]
    [InlineData(0, PI / 4, PI / 4)]
    [InlineData(PI / 4, -PI / 4, -PI / 2)]
    [InlineData(PI / 2, 13 * PI / 2, 0)]
    [InlineData(0, -PI, -PI)]
    public void AngleDiff_ShouldReturnSignedDiff_WhenGivenTwoAngles(double from, double to, double expected)
    {
        var result = MathUtils.AngleDiff(from, to);
        result.Should().BeApproximately(expected, 0.01);
    }

    [Theory]
    [InlineData(1.5, 1.5, 0)]
    [InlineData(0, PI / 4, PI / 4)]
    [InlineData(PI / 4, -PI / 4, PI / 2)]
    [InlineData(PI / 2, 13 * PI / 2, 0)]
    [InlineData(0, -PI, PI)]
    public void AbsAngleDiff_ShouldReturnUnsignedDiff_WhenGivenTwoAngles(double alpha, double beta, double expected)
    {
        var result = MathUtils.AbsAngleDiff(alpha, beta);
        result.Should().BeApproximately(expected, 0.01);
    }
}
