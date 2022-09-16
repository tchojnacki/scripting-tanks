using Backend.Domain.Identifiers;
using Backend.Domain.Rooms.GameStates;

namespace Backend.Domain.Game.AI;

internal abstract class TankAI
{
    protected readonly EID _eid;
    protected readonly PlayingGameState _world;

    protected TankAI(EID eid, PlayingGameState world)
    {
        _eid = eid;
        _world = world;
    }

    public abstract (InputAxes inputAxes, double barrelTarget, bool shouldShoot) ApplyInputs();
}
