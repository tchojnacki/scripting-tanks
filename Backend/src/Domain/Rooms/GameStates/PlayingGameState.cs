using Backend.Domain.Identifiers;
using Backend.Domain.Game;
using Backend.Domain.Game.Controls;
using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Client;
using Backend.Utils.Mappings;
using Backend.Mediation.Requests;

using static System.Math;

namespace Backend.Domain.Rooms.GameStates;

internal sealed class PlayingGameState : GameRoom, IWorld
{
    private const double TickRate = 24;
    private const double PlayerDistance = 20;
    private const double IslandMargin = 1;

    private long _lastUpdate;
    private readonly Dictionary<EID, Entity> _entities;
    private readonly IReadOnlyDictionary<CID, PlayerInputCache> _inputCacheMap;
    private readonly Queue<Entity> _spawnQueue = new();
    private readonly Queue<EID> _destroyQueue = new();

    protected override string Location => "game-playing";

    private PlayingGameState(WaitingGameState previous) : base(previous)
    {
        _lastUpdate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        var playerCount = AllPlayers.Count();
        var step = 2 * PI / playerCount;
        Radius = Floor(
            playerCount == 1
                ? PlayerDistance / 2
                : PlayerDistance / Sqrt(2 - 2 * Cos(2 * PI / playerCount))
        ) + IslandMargin;

        _inputCacheMap = AllPlayers.Select(player => new
        {
            Player = player,
            Cache = new PlayerInputCache()
        }).ToDictionary(o => o.Player.CID, p => p.Cache);

        _entities = AllPlayers.Select((player, i) => (Entity)new Tank(
            world: this,
            playerData: player,
            pos: new Vector(
                Sin(step * i) * (Radius - IslandMargin),
                0,
                Cos(step * i) * (Radius - IslandMargin)),
            pitch: step * i + PI,
            controller: player.IsBot
                ? new BotTankController(player.CID.GetHashCode())
                : new PlayerTankController(_inputCacheMap[player.CID])
        )).ToDictionary(t => t.EID);

        Scoreboard = new(AllPlayers);

        Task.Run(async () => await GameLoop());
    }

    public static PlayingGameState AfterWaiting(WaitingGameState previous) => new(previous);

    public IEnumerable<Entity> Entities => _entities.Values;

    public Scoreboard Scoreboard { get; }

    public double Radius { get; }

    public IEnumerable<Tank> Tanks => _entities.Values.OfType<Tank>();

    private Task SetInputAxes(CID cid, InputAxes inputAxes)
    {
        _inputCacheMap[cid].InputAxes = inputAxes;
        return Task.CompletedTask;
    }

    private Task SetBarrelTarget(CID cid, double barrelTarget)
    {
        _inputCacheMap[cid].BarrelTarget = barrelTarget;
        return Task.CompletedTask;
    }

    private Task Shoot(CID cid)
    {
        _inputCacheMap[cid].EnqueueShoot();
        return Task.CompletedTask;
    }

    public override Task HandleOnMessageAsync(CID cid, IClientMessage message) => message switch
    {
        SetBarrelTargetClientMessage { Data: var barrelTarget } => SetBarrelTarget(cid, barrelTarget),
        SetInputAxesClientMessage { Data: var dto } => SetInputAxes(cid, dto.ToDomain()),
        ShootClientMessage => Shoot(cid),
        _ => base.HandleOnMessageAsync(cid, message)
    };

    public void Spawn(Entity entity) => _spawnQueue.Enqueue(entity);

    public void Destroy(Entity entity)
    {
        _destroyQueue.Enqueue(entity.EID);

        if (entity is Tank)
        {
            foreach (var cid in Scoreboard.Players)
            {
                var eid = EID.FromCID(cid);

                if (eid != entity.EID && _entities.ContainsKey(eid))
                    Scoreboard.Increment(cid);
            }
        }
    }

    private void ResolveLifetimes()
    {
        while (_destroyQueue.TryDequeue(out var eid)) _entities.Remove(eid);
        while (_spawnQueue.TryDequeue(out var entity)) _entities[entity.EID] = entity;
    }

    private void ResolveUpdates(TimeSpan deltaTime)
    {
        foreach (var entity in _entities.Values) entity.Update(deltaTime);
    }

    private void ResolveCollisions()
    {
        foreach (var (eid1, ent1) in _entities)
        {
            foreach (var (eid2, ent2) in _entities)
            {
                if (eid2.ToString().CompareTo(eid1.ToString()) > 0 &&
                    ent1.Pos.Y >= 0 && ent2.Pos.Y >= 0 &&
                    (ent2.Pos - ent1.Pos).Length < ent1.Radius + ent2.Radius)
                {
                    ent1.CollideWith(ent2);
                    ent2.CollideWith(ent1);
                }
            }
        }
    }

    private async Task GameLoop()
    {
        while (Tanks.Count() >= 2)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var deltaTime = TimeSpan.FromMilliseconds(now - _lastUpdate);
            _lastUpdate = now;

            ResolveLifetimes();
            ResolveUpdates(deltaTime);
            ResolveCollisions();

            await _mediator.Send(new BroadcastRoomStateRequest(LID));

            await Task.Delay(TimeSpan.FromSeconds(1 / TickRate));
        }

        await _mediator.Send(new ShowSummaryRequest(LID));
    }

    public override async Task HandleOnLeaveAsync(CID cid)
    {
        await base.HandleOnLeaveAsync(cid);
        _entities.Remove(EID.FromCID(cid));
    }
}