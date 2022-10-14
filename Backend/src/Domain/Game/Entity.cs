using Backend.Domain.Identifiers;

namespace Backend.Domain.Game;

internal abstract class Entity
{
    protected readonly double Mass; // kg
    protected readonly IWorld World;
    private Vector _acceleration; // m/s^2
    protected Vector Velocity; // m/s

    protected Entity(
        IWorld world,
        Eid? eid = null,
        Vector position = default,
        Vector velocity = default,
        double radius = 0,
        double mass = 1)
    {
        World = world;
        Eid = eid ?? Eid.GenerateUnique();
        Position = position;
        Velocity = velocity;
        Radius = radius;
        Mass = mass;
    }

    public Eid Eid { get; }
    public Vector Position { get; private set; }
    public double Radius { get; protected set; }

    private double HighestPoint => Position.Y + Radius;

    public virtual void Update(TimeSpan deltaTime)
    {
        var forces = CalculateForces();
        _acceleration = forces / Mass;
        Velocity += _acceleration * deltaTime.TotalSeconds;
        Position += Velocity * deltaTime.TotalSeconds;

        if (HighestPoint < IWorld.SeaHeight)
            World.Destroy(this);
    }

    public virtual void CollideWith(Entity other)
    {
    }

    protected virtual Vector CalculateForces() => default;
}
