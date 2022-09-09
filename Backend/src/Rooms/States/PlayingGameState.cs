using Backend.Identifiers;
using Backend.Utils.Mappings;
using Backend.Utils.Common;
using Backend.Domain.Game;
using Backend.Contracts.Data;
using Backend.Contracts.Messages.Server;
using Backend.Contracts.Messages.Client;

using static System.Math;
using Backend.Contracts.Messages;

namespace Backend.Rooms.States;

public class PlayingGameState : GameState
{
    private const double TickRate = 24;
    private const double PlayerDistance = 2048;
    private const double IslandMargin = 128;

    private long _lastUpdate;
    private readonly Dictionary<EID, Entity> _entities;
    private readonly IReadOnlyDictionary<CID, string> _nameMap;
    private readonly Dictionary<CID, int> _scoreboard;
    private readonly Queue<Entity> _spawnQueue = new();
    private readonly Queue<EID> _destroyQueue = new();

    public PlayingGameState(GameRoom gameRoom) : base(gameRoom)
    {
        _lastUpdate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        var playerCount = _gameRoom.Players.Count();
        var step = 2 * PI / playerCount;
        Radius = Floor(
            playerCount == 1
                ? PlayerDistance / 2
                : PlayerDistance / Sqrt(2 - 2 * Cos(2 * PI / playerCount))
        ) + IslandMargin;

        _entities = _gameRoom.Players.Select((player, i) => (Entity)new Tank(
            world: this,
            playerData: player,
            pos: new Vector(
                Sin(step * i) * (Radius - IslandMargin),
                0,
                Cos(step * i) * (Radius - IslandMargin)),
            pitch: step * i + PI
        )).ToDictionary(t => t.Eid);

        _nameMap = _gameRoom.Players.ToDictionary(p => p.Cid, p => p.Name);

        _scoreboard = _gameRoom.Players.ToDictionary(p => p.Cid, _ => 0);

        Task.Run(async () => await GameLoop());
    }

    public double Radius { get; }

    public IEnumerable<Tank> Tanks => _entities.Values.OfType<Tank>();

    public override Task HandleOnMessageAsync(CID cid, IClientMessage message)
    {
        var eid = EID.From("EID$" + HashUtils.Hash(cid.Value));
        if (!_entities.ContainsKey(eid)) return Task.CompletedTask;
        var tank = (Tank)_entities[eid];
        switch (message)
        {
            case SetInputAxesClientMessage { Data: var dto }:
                tank.InputAxes = dto.ToDomain();
                break;

            case SetBarrelTargetClientMessage { Data: var barrelTarget }:
                tank.BarrelTarget = barrelTarget;
                break;

            case ShootClientMessage:
                tank.Shoot();
                break;
        }

        return Task.CompletedTask;
    }

    public void Spawn(Entity entity) => _spawnQueue.Enqueue(entity);

    public void Destroy(Entity entity)
    {
        _destroyQueue.Enqueue(entity.Eid);

        if (entity is Tank)
        {
            foreach (var cid in _scoreboard.Keys)
            {
                var eid = EID.From("EID$" + HashUtils.Hash(cid.Value));

                if (eid != entity.Eid && _entities.ContainsKey(eid))
                    _scoreboard[cid]++;
            }
        }
    }

    private void ResolveCollisions()
    {
        foreach (var (eid1, ent1) in _entities)
        {
            foreach (var (eid2, ent2) in _entities)
            {
                if (eid2.Value.CompareTo(eid1.Value) > 0 &&
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

            while (_destroyQueue.TryDequeue(out var eid)) _entities.Remove(eid);
            while (_spawnQueue.TryDequeue(out var entity)) _entities[entity.Eid] = entity;
            foreach (var entity in _entities.Values) entity.Update(deltaTime);
            ResolveCollisions();

            await _gameRoom.BroadcastMessageAsync(new RoomStateServerMessage { Data = RoomState });

            await Task.Delay(TimeSpan.FromSeconds(1 / TickRate));
        }

        await _gameRoom.ShowSummary(_scoreboard);
    }

    public override GamePlayingStateDto RoomState => new()
    {
        Radius = Radius,
        Entities = _entities.Values.Select(e => e.ToDto()).ToList(),
        Scoreboard = _scoreboard
            .OrderByDescending(p => p.Value)
            .Select(p => new ScoreboardEntryDto
            {
                Cid = p.Key.Value,
                Name = _nameMap[p.Key],
                Score = p.Value,
            })
            .ToList()
    };
}
