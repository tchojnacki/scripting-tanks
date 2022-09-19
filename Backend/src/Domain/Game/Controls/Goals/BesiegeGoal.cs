using static Backend.Utils.Common.MathUtils;

namespace Backend.Domain.Game.Controls.Goals;

internal sealed class BesiegeGoal : IGoal
{
    public bool CanActivate(GoalContext context) => context.TargetOffset.Length <= context.Traits.Range;

    public TankControlsStatus CarryOut(GoalContext context) => new()
    {
        InputAxes = default,
        BarrelTarget = context.TargetOffset.Pitch,
        ShouldShoot = AbsAngleDiff(context.Self.BarrelPitch, context.TargetOffset.Pitch) <= context.Traits.Accuracy
    };
}
