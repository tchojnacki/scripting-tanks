using static System.Math;
using static Backend.Utils.Common.MathUtils;

namespace Backend.Domain.Game.Controls.Goals;

internal sealed class StraightenCourseGoal : IGoal
{
    public bool CanActivate(GoalContext context)
        => AbsAngleDiff(context.Self.Pitch, context.TargetOffset.Pitch) > PI / 2;

    public TankControlsStatus CarryOut(GoalContext context)
        => TankControlsStatus.Moving(context.Self, new()
        {
            Vertical = 1,
            Horizontal = 1
        });
}
