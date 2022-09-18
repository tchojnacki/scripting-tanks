namespace Backend.Domain.Game.Controls;

internal interface ITankController
{
    TankControlsStatus FetchControlsStatus(Tank self, IWorld world);
}
