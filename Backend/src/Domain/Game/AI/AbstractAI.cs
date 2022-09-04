using Backend.Identifiers;
using Backend.Rooms.States;

namespace Backend.Domain.Game.AI;

public abstract class AbstractAI
{
    protected readonly EID _eid;
    protected readonly PlayingGameState _world;

    protected AbstractAI(EID eid, PlayingGameState world)
    {
        _eid = eid;
        _world = world;
    }
}
