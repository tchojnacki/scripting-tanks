using Backend.Domain.Identifiers;

namespace Backend.Domain.Game;

internal sealed class Bullet : Entity
{
    private const double BulletSpeed = 10.24;
    private const double RadiusGrowthTempo = 0.64;
    private const double MaxBulletRadius = 0.16;
    private static readonly Vector GravityPull = new(0, -2, 0);

    public Bullet(
        IWorld world,
        CID ownerCid,
        double direction,
        Vector pos) : base(
            world,
            pos: pos,
            vel: Vector.UnitWithPitch(direction) * BulletSpeed,
            radius: 0)
    {
        OwnerCID = ownerCid;
    }

    public CID OwnerCID { get; }

    protected override Vector CalculateForces()
        => GravityPull * _mass;

    public override void Update(TimeSpan deltaTime)
    {
        base.Update(deltaTime);
        Radius = Math.Min(Radius + RadiusGrowthTempo * deltaTime.TotalSeconds, MaxBulletRadius);
    }
}
