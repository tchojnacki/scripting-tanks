namespace Backend.Domain.Game.Controls.Goals;

internal interface IGoal
{
    public bool CanActivate(GoalContext context);
    public TankControlsStatus CarryOut(GoalContext context);
}
