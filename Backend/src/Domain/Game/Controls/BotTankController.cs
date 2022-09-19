using Backend.Domain.Game.Controls.Goals;

namespace Backend.Domain.Game.Controls;

internal sealed class BotTankController : ITankController
{
    private readonly IReadOnlyList<IGoal> _goalPriorityList = new List<IGoal>
    {
        new BesiegeGoal(),
        new StraightenCourseGoal(),
        new ChaseGoal()
    };

    public TankControlsStatus FetchControlsStatus(NavigationContext context)
    {
        var target = context.World.Tanks
            .Where(t => t.EID != context.Self.EID)
            .MinBy(t => (context.Self.Pos - t.Pos).Length);
        if (target is null) return TankControlsStatus.Idle(context.Self);
        var targetOffset = target.Pos - context.Self.Pos;

        var goalContext = new GoalContext(context.Self, targetOffset, context.World);
        return _goalPriorityList.First(g => g.CanActivate(goalContext)).CarryOut(goalContext);
    }
}
