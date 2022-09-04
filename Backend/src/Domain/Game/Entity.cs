using Backend.Identifiers;
using Backend.Utils.Common;
using Backend.Rooms.States;

namespace Backend.Domain.Game;

public abstract class Entity
{
    private Vector _vel;
    private Vector _acc;
    private double _mass;

    protected Entity(
        PlayingGameState world,
        EID? eid = null,
        Vector pos = default,
        Vector vel = default,
        double radius = 0,
        double mass = 1)
    {
        World = world;
        Eid = eid ?? EID.From("EID$" + HashUtils.RandomHash());
        Pos = pos;
        _vel = vel;
        Radius = radius;
        _mass = mass;
    }

    public PlayingGameState World { get; }
    public EID Eid { get; }
    public Vector Pos { get; }
    public double Radius { get; }
}
