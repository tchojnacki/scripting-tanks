namespace Backend.Domain.Game.Controls;

internal sealed record TankControlsStatus
{
    public InputAxes InputAxes { get; init; }
    public double BarrelTarget { get; init; }
    public bool ShouldShoot { get; init; }

    public static TankControlsStatus Idle(Tank tank) => new()
    {
        InputAxes = default,
        BarrelTarget = tank.BarrelPitch,
        ShouldShoot = false
    };

    public static TankControlsStatus Moving(Tank tank, InputAxes inputAxes) => new()
    {
        InputAxes = inputAxes,
        BarrelTarget = tank.Pitch,
        ShouldShoot = false
    };
}
