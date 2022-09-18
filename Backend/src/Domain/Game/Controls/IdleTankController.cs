namespace Backend.Domain.Game.Controls;

internal sealed class IdleTankController : ITankController
{
    public TankControlsStatus FetchControlsStatus(Tank self, IWorld world)
        => TankControlsStatus.Idle(self);
}
