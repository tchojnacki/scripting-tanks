using Backend.Identifiers;
using Backend.Rooms.States;

namespace Backend.Domain.Game.AI;

public class SimpleAI : TankAI
{
    public SimpleAI(EID eid, PlayingGameState world) : base(eid, world) { }
}
