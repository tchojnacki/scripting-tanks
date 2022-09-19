namespace Backend.Domain.Game.Controls;

internal sealed record AITraits
{
    public int PreferredTarget { get; init; }
    public double Accuracy { get; init; }
    public double Range { get; init; }
    public double StraightenThreshold { get; init; }
    public InputAxes StraightenAxes { get; init; }
}
