namespace Backend.Domain.Game.Controls;

internal interface ITankController
{
    TankControlsStatus FetchControlsStatus(NavigationContext context);
}
