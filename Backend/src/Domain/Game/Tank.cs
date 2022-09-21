using Backend.Domain.Game.Controls;

using static System.Math;
using static Backend.Utils.Common.MathUtils;

namespace Backend.Domain.Game;

internal sealed class Tank : Entity
{
    private const double CDrag = 10_000; // kg/m
    private const double CRollResistance = 5_000; // kg/s
    private const double EngineForce = 30_000; // kg*m/s^2
    private const double ReverseMultiplier = 0.25; // unitless
    private const double TurnDegree = PI / 12; // rad
    private const double BarrelTurnSpeed = PI / 2; // rad/s
    private const double BarrelHeight = 0.6; // m
    private const double TankRadius = 0.6; // m
    private const double TankMass = 10_000; // kg
    private const double GravityOutwardPush = 10; // m/s^2
    private static readonly TimeSpan ShootCooldown = TimeSpan.FromSeconds(2);

    private readonly ITankController _controller;
    private double _barrelTarget; // rad
    private InputAxes _inputAxes;
    private long _lastShot; // ms

    public Tank(
        IWorld world,
        PlayerData playerData,
        Vector position,
        double pitch,
        ITankController controller) : base(
            world: world,
            eid: Identifiers.EID.FromCID(playerData.CID),
            position: position,
            radius: TankRadius,
            mass: TankMass)
    {
        PlayerData = playerData;
        Pitch = pitch;
        BarrelPitch = pitch;
        _barrelTarget = pitch;
        _lastShot = 0;
        _controller = controller;
    }

    public PlayerData PlayerData { get; }
    public double Pitch { get; private set; } // rad
    public double BarrelPitch { get; private set; } // rad

    public override void Update(TimeSpan deltaTime)
    {
        HandleInput();

        var turnAngle = -_inputAxes.Horizontal * Sign(_inputAxes.Vertical) * TurnDegree;

        if (Abs(turnAngle) > 0.01)
        {
            var turnRadius = 2 * Radius / Sin(turnAngle);
            var omega = _velocity.Length / turnRadius;
            Pitch += omega * deltaTime.TotalSeconds;
        }

        var barrelDiff = AngleDiff(BarrelPitch, _barrelTarget);
        BarrelPitch += CopySign(Min(BarrelTurnSpeed * deltaTime.TotalSeconds, Abs(barrelDiff)), barrelDiff);

        base.Update(deltaTime);
    }

    private void HandleInput()
    {
        var controlsStatus = _controller.FetchControlsStatus(new(this, _world));
        _inputAxes = controlsStatus.InputAxes;
        _barrelTarget = controlsStatus.BarrelTarget;
        if (controlsStatus.ShouldShoot) Shoot();
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

        var engineForce = (Vector.Dot(_velocity, u) > 0 ? 1 : ReverseMultiplier) * EngineForce * _inputAxes.Vertical;

        var fTraction = u * engineForce;
        var fDrag = -CDrag * _velocity * _velocity.Length;
        var fRollResist = -CRollResistance * _velocity;
        var fGravity = CalculateGravityForce();

        return fTraction + fDrag + fRollResist + fGravity;
    }

    private Vector CalculateGravityForce()
    {
        var fGravity = default(Vector);

        if (Position.Length > _world.Radius)
        {
            fGravity += IWorld.Gravity;
            if (Position.Length < _world.Radius + Radius)
                fGravity += GravityOutwardPush * Position.Normalized;
        }

        return fGravity * _mass;
    }

    private void Shoot()
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        if (TimeSpan.FromMilliseconds(now - _lastShot) >= ShootCooldown)
        {
            _lastShot = now;

            _world.Spawn(new Bullet(
                world: _world,
                ownerCid: PlayerData.CID,
                direction: BarrelPitch,
                position: Position + new Vector(0, BarrelHeight, 0)
            ));
        }
    }
}
