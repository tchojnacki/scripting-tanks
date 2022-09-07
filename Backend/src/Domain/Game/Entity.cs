using Backend.Identifiers;
using Backend.Utils.Common;
using Backend.Rooms.States;

namespace Backend.Domain.Game;

public abstract class Entity
{
    private const double SeaHeight = -100;

    protected readonly PlayingGameState _world;
    protected Vector _vel;
    private Vector _acc;
    protected readonly double _mass;

    protected Entity(
        PlayingGameState world,
        EID? eid = null,
        Vector pos = default,
        Vector vel = default,
        double radius = 0,
        double mass = 1)
    {
        _world = world;
        Eid = eid ?? EID.From("EID$" + HashUtils.RandomHash());
        Pos = pos;
        _vel = vel;
        Radius = radius;
        _mass = mass;
    }

    public EID Eid { get; }
    public Vector Pos { get; private set; }
    public double Radius { get; protected set; }

    private double HighestPoint => Pos.Y + Radius;

    public virtual void Update(TimeSpan deltaTime)
    {
        var forces = CalculateForces();
        _acc = forces / _mass;
        _vel += _acc * deltaTime.TotalSeconds;
        Pos += _vel * deltaTime.TotalSeconds;

        if (HighestPoint < SeaHeight)
            _world.Destroy(this);
    }

    protected virtual Vector CalculateForces() => default;
}
