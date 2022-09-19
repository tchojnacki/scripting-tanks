namespace Backend.Domain.Game.Controls;

internal sealed class IdleTankController : ITankController
{
    public TankControlsStatus FetchControlsStatus(NavigationContext context)
        => TankControlsStatus.Idle(context.Self);
}
