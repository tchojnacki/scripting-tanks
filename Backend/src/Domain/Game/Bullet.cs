using Backend.Domain.Identifiers;
using Backend.Domain.Rooms.States;

namespace Backend.Domain.Game;

public class Bullet : Entity
{
    private const double BulletSpeed = 1024;
    private const double RadiusGrowthTempo = 64;
    private const double MaxBulletRadius = 16;
    private static readonly Vector GravityPull = new(0, -200, 0);

    public Bullet(
        PlayingGameState world,
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
