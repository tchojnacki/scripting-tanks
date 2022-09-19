namespace Backend.Domain.Game.Controls.Goals;

internal interface IGoal
{
    public abstract bool CanActivate(GoalContext context);
    public abstract TankControlsStatus CarryOut(GoalContext context);
}
