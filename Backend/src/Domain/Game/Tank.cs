using Backend.Identifiers;
using Backend.Rooms.States;
using Backend.Domain.Game.AI;
using Backend.Utils.Common;

namespace Backend.Domain.Game;

public class Tank : Entity
{
    private const double TankRadius = 64;
    private const double TankMass = 10_000;

    private readonly TankAI? _ai;
    private double _barrelPitch;
    private long _lastShot;

    public Tank(
        PlayingGameState world,
        PlayerData playerData,
        Vector pos,
        double pitch) : base(
            world: world,
            eid: EID.From("EID$" + HashUtils.Hash(playerData.Cid.Value)),
            pos: pos,
            radius: TankRadius,
            mass: TankMass)
    {
        PlayerData = playerData;
        Pitch = pitch;
        BarrelTarget = pitch;
        _barrelPitch = pitch;
        _lastShot = 0;
        _ai = playerData.IsBot ? new SimpleAI(Eid, world) : null;
    }

    public PlayerData PlayerData { get; }
    public double Pitch { get; }
    public double BarrelTarget { get; }
    public InputAxes InputAxes { get; }
}
