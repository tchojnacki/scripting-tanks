using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Client;
using Backend.Domain.Game;
using Backend.Domain.Game.Controls;
using Backend.Domain.Identifiers;
using Backend.Mediation.Requests;
using Backend.Utils.Mappings;
using static System.Math;

namespace Backend.Domain.Rooms.GameStates;

internal sealed class PlayingGameState : GameRoom, IWorld
{
    private const double TickRate = 24; // Hz
    private const double PlayerDistance = 20; // m
    private const double IslandMargin = 1; // m
    private readonly Queue<Eid> _destroyQueue = new();
    private readonly Dictionary<Eid, Entity> _entities;
    private readonly IReadOnlyDictionary<Cid, PlayerInputCache> _inputCacheMap;
    private readonly Queue<Entity> _spawnQueue = new();

    private long _lastUpdate;

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
        }).ToDictionary(o => o.Player.Cid, p => p.Cache);

        _entities = AllPlayers.Select((player, i) => (Entity)new Tank(
            this,
            player,
            new(
                Sin(step * i) * (Radius - IslandMargin),
                0,
                Cos(step * i) * (Radius - IslandMargin)),
            step * i + PI,
            player.IsBot
                ? new BotTankController(player.Cid.GetHashCode())
                : new PlayerTankController(_inputCacheMap[player.Cid])
        )).ToDictionary(t => t.Eid);

        Scoreboard = new(AllPlayers);

        Task.Run(async () => await GameLoop());
    }

    protected override string Location => "game-playing";

    public IEnumerable<Entity> Entities => _entities.Values;

    public Scoreboard Scoreboard { get; }

    public double Radius { get; }

    public IEnumerable<Tank> Tanks => _entities.Values.OfType<Tank>();

    public void Spawn(Entity entity) => _spawnQueue.Enqueue(entity);

    public void Destroy(Entity entity)
    {
        _destroyQueue.Enqueue(entity.Eid);

        if (entity is Tank)
            foreach (var cid in Scoreboard.Players)
            {
                var eid = Eid.FromCid(cid);

                if (eid != entity.Eid && _entities.ContainsKey(eid))
                    Scoreboard.Increment(cid);
            }
    }

    public static PlayingGameState AfterWaiting(WaitingGameState previous) => new(previous);

    private Task SetInputAxes(Cid cid, InputAxes inputAxes)
    {
        _inputCacheMap[cid].InputAxes = inputAxes;
        return Task.CompletedTask;
    }

    private Task SetBarrelTarget(Cid cid, double barrelTarget)
    {
        _inputCacheMap[cid].BarrelTarget = barrelTarget;
        return Task.CompletedTask;
    }

    private Task Shoot(Cid cid)
    {
        _inputCacheMap[cid].EnqueueShoot();
        return Task.CompletedTask;
    }

    public override Task HandleOnMessageAsync(Cid cid, IClientMessage message) => message switch
    {
        SetBarrelTargetClientMessage { Data: var barrelTarget } => SetBarrelTarget(cid, barrelTarget),
        SetInputAxesClientMessage { Data: var dto } => SetInputAxes(cid, dto.ToDomain()),
        ShootClientMessage => Shoot(cid),
        _ => base.HandleOnMessageAsync(cid, message)
    };

    private void ResolveLifetimes()
    {
        while (_destroyQueue.TryDequeue(out var eid)) _entities.Remove(eid);
        while (_spawnQueue.TryDequeue(out var entity)) _entities[entity.Eid] = entity;
    }

    private void ResolveUpdates(TimeSpan deltaTime)
    {
        foreach (var entity in _entities.Values) entity.Update(deltaTime);
    }

    private void ResolveCollisions()
    {
        foreach (var (eid1, ent1) in _entities)
        foreach (var (eid2, ent2) in _entities)
            if (string.Compare(eid2.ToString(), eid1.ToString(), StringComparison.InvariantCulture) > 0 &&
                ent1.Position.Y >= 0 && ent2.Position.Y >= 0 &&
                (ent2.Position - ent1.Position).Length < ent1.Radius + ent2.Radius)
            {
                ent1.CollideWith(ent2);
                ent2.CollideWith(ent1);
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

            await Mediator.Send(new BroadcastRoomStateRequest(Lid));

            await Task.Delay(TimeSpan.FromSeconds(1 / TickRate));
        }

        await Mediator.Send(new ShowSummaryRequest(Lid));
    }

    public override async Task HandleOnLeaveAsync(Cid cid)
    {
        await base.HandleOnLeaveAsync(cid);
        _entities.Remove(Eid.FromCid(cid));
    }
}
