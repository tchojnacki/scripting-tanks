using Backend.Domain.Identifiers;

namespace Backend.Domain.Game;

internal sealed class Bullet : Entity
{
    private const double BulletSpeed = 10; // m/s
    private const double RadiusGrowthTempo = 0.5; // m/s
    private const double MaxBulletRadius = 0.15; // m

    public Bullet(
        IWorld world,
        CID ownerCid,
        double direction,
        Vector position) : base(
            world,
            position: position,
            velocity: Vector.UnitWithPitch(direction) * BulletSpeed,
            radius: 0)
    {
        OwnerCID = ownerCid;
    }

    public CID OwnerCID { get; }

    protected override Vector CalculateForces() => IWorld.Gravity * _mass;

    public override void Update(TimeSpan deltaTime)
    {
        base.Update(deltaTime);
        Radius = Math.Min(Radius + RadiusGrowthTempo * deltaTime.TotalSeconds, MaxBulletRadius);
    }
}
