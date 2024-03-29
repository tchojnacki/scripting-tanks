namespace Backend.Domain.Game.Controls;

internal sealed class PlayerTankController : ITankController
{
    private readonly PlayerInputCache _playerInputCache;

    public PlayerTankController(PlayerInputCache playerInputCache) => _playerInputCache = playerInputCache;

    public TankControlsStatus FetchControlsStatus(NavigationContext context) => new()
    {
        InputAxes = _playerInputCache.InputAxes,
        BarrelTarget = _playerInputCache.BarrelTarget,
        ShouldShoot = _playerInputCache.DequeueShot()
    };
}
