using Backend.Domain.Rooms.GameStates;
using Backend.Domain.Game.Controls;

using static System.Math;

namespace Backend.Domain.Game;

internal sealed class Tank : Entity
{
    private const double CDrag = 100;
    private const double CRollResist = 5_000;
    private const double EngineForce = 3_000_000;
    private const double ReverseMult = 0.25;
    private const double TurnDegree = PI / 12;
    private const double BarrelTurnSpeed = PI / 2;
    private const double BarrelHeight = 60;
    private const double ShootCooldown = 2;
    private const double TankRadius = 64;
    private const double TankMass = 10_000;
    private const double GravityOutwardPush = 2048;
    private static readonly Vector GravityAcceleration = new(0, -2048, 0);

    private readonly ITankController? _controls;
    private long _lastShot;

    public Tank(
        PlayingGameState world,
        PlayerData playerData,
        Vector pos,
        double pitch) : base(
            world: world,
            eid: Identifiers.EID.FromCID(playerData.CID),
            pos: pos,
            radius: TankRadius,
            mass: TankMass)
    {
        PlayerData = playerData;
        Pitch = pitch;
        BarrelTarget = pitch;
        BarrelPitch = pitch;
        _lastShot = 0;
        _controls = playerData.IsBot ? new BotControlledTank() : null;
    }

    public PlayerData PlayerData { get; }
    public double Pitch { get; private set; }
    public double BarrelPitch { get; private set; }
    public double BarrelTarget { get; set; }
    public InputAxes InputAxes { get; set; }

    public override void Update(TimeSpan deltaTime)
    {
        if (_controls is not null)
        {
            var controlsStatus = _controls.FetchControlsStatus(this, _world);
            InputAxes = controlsStatus.InputAxes;
            BarrelTarget = controlsStatus.BarrelTarget;
            if (controlsStatus.ShouldShoot) Shoot();
        }

        var turnAngle = -InputAxes.Horizontal * Sign(InputAxes.Vertical) * TurnDegree;

        if (Abs(turnAngle) > 0.01)
        {
            var turnRadius = 2 * Radius / Sin(turnAngle);
            var omega = _vel.Length / turnRadius;
            Pitch += omega * deltaTime.TotalSeconds;
        }

        var barrelDiff = Atan2(Sin(BarrelTarget - BarrelPitch), Cos(BarrelTarget - BarrelPitch));

        BarrelPitch += CopySign(Min(BarrelTurnSpeed * deltaTime.TotalSeconds, Abs(barrelDiff)), barrelDiff);

        base.Update(deltaTime);
    }

    public override void CollideWith(Entity other)
    {
        switch (other)
        {
            case Bullet bullet when bullet.OwnerCID != PlayerData.CID:
            case Tank:
                _world.Destroy(this);
                _world.Destroy(other);
                break;
        }
    }

    protected override Vector CalculateForces()
    {
        var u = new Vector(Sin(Pitch), 0, Cos(Pitch));

        var engineForce = (Vector.Dot(_vel, u) > 0 ? 1 : ReverseMult) * EngineForce * InputAxes.Vertical;

        var fTraction = u * engineForce;
        var fDrag = -CDrag * _vel * _vel.Length;
        var fRollResist = -CRollResist * _vel;
        var fGravity = CalculateGravityForce();

        return fTraction + fDrag + fRollResist + fGravity;
    }

    private Vector CalculateGravityForce()
    {
        var fGravity = default(Vector);

        if (Pos.Length > _world.Radius)
        {
            fGravity += GravityAcceleration;
            if (Pos.Length < _world.Radius + Radius)
                fGravity += GravityOutwardPush * Pos.Normalized;
        }

        return fGravity * _mass;
    }

    public void Shoot()
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        if ((now - _lastShot) / 1000 >= ShootCooldown)
        {
            _lastShot = now;

            _world.Spawn(new Bullet(
                world: _world,
                ownerCid: PlayerData.CID,
                direction: BarrelPitch,
                pos: Pos + new Vector(0, BarrelHeight, 0)
            ));
        }
    }
}
