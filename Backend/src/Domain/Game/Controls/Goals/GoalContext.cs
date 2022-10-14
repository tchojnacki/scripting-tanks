namespace Backend.Domain.Game.Controls.Goals;

internal sealed record GoalContext(Tank Self, AiTraits Traits, Vector TargetOffset);
