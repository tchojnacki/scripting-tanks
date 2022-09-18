using static System.Math;

namespace Backend.Domain.Game.Controls;

internal sealed class BotControlledTank : ITankController
{
    public TankControlsStatus FetchControlsStatus(Tank self, IWorld world)
    {
        var target = world.Tanks.Where(t => t.EID != self.EID).MinBy(t => (self.Pos - t.Pos).Length);
        if (target is null) return TankControlsStatus.Idle(self);

        var offset = target.Pos - self.Pos;
        var facing = Vector.UnitWithPitch(self.Pitch);

        var inputAxes = new InputAxes
        {
            Vertical = Clamp(Vector.Dot(offset, facing), -1, 1),
            Horizontal = Clamp(Vector.Cross(offset, facing).Z, -1, 1)
        };
        var barrelTarget = Atan2(offset.X, offset.Z);
        var shouldShoot = offset.Length < 512;

        return new()
        {
            InputAxes = inputAxes,
            BarrelTarget = barrelTarget,
            ShouldShoot = shouldShoot
        };
    }
}
