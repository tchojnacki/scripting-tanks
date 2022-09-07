using Backend.Identifiers;
using Backend.Rooms.States;

namespace Backend.Domain.Game.AI;

public abstract class TankAI
{
    protected readonly EID _eid;
    protected readonly PlayingGameState _world;

    protected TankAI(EID eid, PlayingGameState world)
    {
        _eid = eid;
        _world = world;
    }

    public abstract (InputAxes inputAxes, double pitch, bool shouldShoot) ApplyInputs();
}
