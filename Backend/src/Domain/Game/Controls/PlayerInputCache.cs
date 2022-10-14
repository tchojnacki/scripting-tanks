namespace Backend.Domain.Game.Controls;

internal sealed class PlayerInputCache
{
    private bool _shouldShoot;

    public InputAxes InputAxes { get; set; }
    public double BarrelTarget { get; set; }

    public void EnqueueShoot()
    {
        _shouldShoot = true;
    }

    public bool DequeueShot()
    {
        var result = _shouldShoot;
        _shouldShoot = false;
        return result;
    }
}
