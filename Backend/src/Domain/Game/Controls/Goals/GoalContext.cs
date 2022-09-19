namespace Backend.Domain.Game.Controls;

internal sealed record GoalContext(Tank Self, AITraits Traits, Vector TargetOffset);
