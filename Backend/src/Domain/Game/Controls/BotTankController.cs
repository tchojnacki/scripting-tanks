using Backend.Domain.Game.Controls.Goals;

using static System.Math;

namespace Backend.Domain.Game.Controls;

internal sealed class BotTankController : ITankController
{
    private static readonly IReadOnlyList<IGoal> GoalPriorityList = new List<IGoal>
    {
        new BesiegeGoal(),
        new StraightenCourseGoal(),
        new ChaseGoal()
    };

    private readonly AITraits _traits;

    public BotTankController(int aiSeed)
    {
        var random = new Random(aiSeed);

        _traits = new()
        {
            PreferredTarget = random.Next(2),
            Accuracy = random.NextDouble() * PI / 6,
            Range = 2.5 + random.NextDouble() * 6,
            StraightenThreshold = PI * 3 / 8 + random.NextDouble() * PI / 4,
            StraightenAxes = new()
            {
                Vertical = random.Next(2) == 1 ? 1 : -1,
                Horizontal = random.Next(2) == 1 ? 1 : -1,
            }
        };
    }

    public TankControlsStatus FetchControlsStatus(NavigationContext context)
    {
        var otherTanks = context.World.Tanks.Where(t => t.EID != context.Self.EID).ToList();
        if (otherTanks.Count == 0) return TankControlsStatus.Idle(context.Self);
        var prioritizedTargets = otherTanks.Where(t => (context.Self.Pos - t.Pos).Length <= _traits.Range).ToList();
        var target = prioritizedTargets.Any()
            ? prioritizedTargets[_traits.PreferredTarget % prioritizedTargets.Count]
            : otherTanks[_traits.PreferredTarget % otherTanks.Count];

        var goalContext = new GoalContext(context.Self, _traits, target.Pos - context.Self.Pos);
        return GoalPriorityList.First(g => g.CanActivate(goalContext)).CarryOut(goalContext);
    }
}
