using Backend.Domain.Identifiers;

namespace Backend.Domain.Game;

internal sealed class Bullet : Entity
{
    private const double BulletSpeed = 10; // m/s
    private const double RadiusGrowthTempo = 0.5; // m/s
    private const double MaxBulletRadius = 0.15; // m

    public Bullet(
        IWorld world,
        Cid ownerCid,
        double direction,
        Vector position) : base(
        world,
        position: position,
        velocity: Vector.UnitWithPitch(direction) * BulletSpeed,
        radius: 0) =>
        OwnerCid = ownerCid;

    public Cid OwnerCid { get; }

    protected override Vector CalculateForces() => IWorld.Gravity * Mass;

    public override void Update(TimeSpan deltaTime)
    {
        base.Update(deltaTime);
        Radius = Math.Min(Radius + RadiusGrowthTempo * deltaTime.TotalSeconds, MaxBulletRadius);
    }
}
