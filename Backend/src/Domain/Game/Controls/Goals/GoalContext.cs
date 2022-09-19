namespace Backend.Domain.Game.Controls;

internal sealed record GoalContext(Tank Self, Vector TargetOffset, IWorld World);
