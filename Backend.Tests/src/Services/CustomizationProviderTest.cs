using System.Text.RegularExpressions;
using Backend.Services;

namespace Backend.Tests.Services;

public class CustomizationProviderTest
{
    private static readonly Regex ColorRegex = new("^#[0-9a-f]{6}$", RegexOptions.Compiled);
    
    private readonly CustomizationProvider _sut;

    public CustomizationProviderTest() => _sut = new();
    
    [Fact]
    public void AssignDisplayName_ShouldAssignATwoPartAnimalName_WhenCalled()
    {
        var name = _sut.AssignDisplayName();

        name.Should().NotBeNullOrEmpty();
        name.Should().Contain(" ");
    }

    [Fact]
    public void AssignTankColors_ShouldAssignCorrectHexColors_WhenCalled()
    {
        foreach (var _ in Enumerable.Range(0, 100))
        {
            var colors = _sut.AssignTankColors();
            
            colors.TankColor.Should().MatchRegex(ColorRegex);
            colors.TurretColor.Should().MatchRegex(ColorRegex);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(42)]
    [InlineData(-5)]
    public void AssignTankColors_ShouldAssignSameColors_WhenSameSeedIsUsed(int seed)
    {
        var color1 = _sut.AssignTankColors(seed);
        var color2 = _sut.AssignTankColors(seed);
        
        color1.Should().Be(color2);
    }
}
