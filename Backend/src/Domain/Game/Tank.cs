using Backend.Domain.Game.Controls;
using Backend.Domain.Identifiers;
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
        world,
        Eid.FromCid(playerData.Cid),
        position,
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
            var omega = Velocity.Length / turnRadius;
            Pitch += omega * deltaTime.TotalSeconds;
        }

        var barrelDiff = AngleDiff(BarrelPitch, _barrelTarget);
        BarrelPitch += CopySign(Min(BarrelTurnSpeed * deltaTime.TotalSeconds, Abs(barrelDiff)), barrelDiff);

        base.Update(deltaTime);
    }

    private void HandleInput()
    {
        var controlsStatus = _controller.FetchControlsStatus(new(this, World));
        _inputAxes = controlsStatus.InputAxes;
        _barrelTarget = controlsStatus.BarrelTarget;
        if (controlsStatus.ShouldShoot) Shoot();
    }

    public override void CollideWith(Entity other)
    {
        switch (other)
        {
            case Bullet bullet when bullet.OwnerCid != PlayerData.Cid:
            case Tank:
                World.Destroy(this);
                World.Destroy(other);
                break;
        }
    }

    protected override Vector CalculateForces()
    {
        var u = new Vector(Sin(Pitch), 0, Cos(Pitch));

        var engineForce = (Vector.Dot(Velocity, u) > 0 ? 1 : ReverseMultiplier) * EngineForce * _inputAxes.Vertical;

        var fTraction = u * engineForce;
        var fDrag = -CDrag * Velocity * Velocity.Length;
        var fRollResist = -CRollResistance * Velocity;
        var fGravity = CalculateGravityForce();

        return fTraction + fDrag + fRollResist + fGravity;
    }

    private Vector CalculateGravityForce()
    {
        if (Position.Length <= World.Radius) return default;

        var fGravity = IWorld.Gravity;
        if (Position.Length <= World.Radius + Radius)
            fGravity += GravityOutwardPush * Position.Normalized;

        return fGravity * Mass;
    }

    private void Shoot()
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        if (TimeSpan.FromMilliseconds(now - _lastShot) < ShootCooldown) return;
        _lastShot = now;

        World.Spawn(new Bullet(
            World,
            PlayerData.Cid,
            BarrelPitch,
            Position + new Vector(0, BarrelHeight, 0)
        ));
    }
}
