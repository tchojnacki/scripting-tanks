using Backend.Domain.Identifiers;

namespace Backend.Domain.Game;

internal abstract class Entity
{
    protected readonly IWorld _world;
    protected Vector _velocity; // m/s
    private Vector _acceleration; // m/s^2
    protected readonly double _mass; // kg

    protected Entity(
        IWorld world,
        EID? eid = null,
        Vector position = default,
        Vector velocity = default,
        double radius = 0,
        double mass = 1)
    {
        _world = world;
        EID = eid ?? EID.GenerateUnique();
        Position = position;
        _velocity = velocity;
        Radius = radius;
        _mass = mass;
    }

    public EID EID { get; }
    public Vector Position { get; private set; }
    public double Radius { get; protected set; }

    private double HighestPoint => Position.Y + Radius;

    public virtual void Update(TimeSpan deltaTime)
    {
        var forces = CalculateForces();
        _acceleration = forces / _mass;
        _velocity += _acceleration * deltaTime.TotalSeconds;
        Position += _velocity * deltaTime.TotalSeconds;

        if (HighestPoint < IWorld.SeaHeight)
            _world.Destroy(this);
    }

    public virtual void CollideWith(Entity other) { }

    protected virtual Vector CalculateForces() => default;
}
