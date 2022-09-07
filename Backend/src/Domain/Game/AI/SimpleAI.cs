using Backend.Identifiers;
using Backend.Rooms.States;

using static System.Math;

namespace Backend.Domain.Game.AI;

public class SimpleAI : TankAI
{
    public SimpleAI(EID eid, PlayingGameState world) : base(eid, world) { }

    public override (InputAxes inputAxes, double pitch, bool shouldShoot) ApplyInputs()
    {
        var tanks = _world.Tanks.ToList();
        var myTank = tanks.Find(t => t.Eid == _eid)!;
        var target = tanks.Where(t => t.Eid != _eid).MinBy(t => (myTank.Pos - t.Pos).Length)!;
        var offset = target.Pos - myTank.Pos;
        var facing = Vector.UnitWithPitch(myTank.Pitch);

        var inputAxes = new InputAxes
        {
            Vertical = Clamp(Vector.Dot(offset, facing), -1, 1),
            Horizontal = Clamp(Vector.Cross(offset, facing).Z, -1, 1)
        };
        var pitch = Atan2(offset.X, offset.Z);
        var shouldShoot = offset.Length < 512;

        return (inputAxes, pitch, shouldShoot);
    }
}
