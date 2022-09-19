namespace Backend.Domain.Game.Controls.Goals;

internal sealed class ChaseGoal : IGoal
{
    public bool CanActivate(GoalContext context) => true;

    public TankControlsStatus CarryOut(GoalContext context)
    {
        var targetDir = context.TargetOffset.Normalized;
        var facingDir = Vector.UnitWithPitch(context.Self.Pitch);

        return TankControlsStatus.Moving(context.Self, new InputAxes
        {
            Vertical = Vector.Dot(targetDir, facingDir),
            Horizontal = Vector.Cross(targetDir, facingDir).Y
        });
    }
}
