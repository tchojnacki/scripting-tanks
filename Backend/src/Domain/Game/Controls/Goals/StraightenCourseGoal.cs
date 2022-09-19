using static Backend.Utils.Common.MathUtils;

namespace Backend.Domain.Game.Controls.Goals;

internal sealed class StraightenCourseGoal : IGoal
{
    public bool CanActivate(GoalContext context)
        => AbsAngleDiff(context.Self.Pitch, context.TargetOffset.Pitch) > context.Traits.StraightenThreshold;

    public TankControlsStatus CarryOut(GoalContext context)
        => TankControlsStatus.Moving(context.Self, context.Traits.StraightenAxes);
}
