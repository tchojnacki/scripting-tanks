using Backend.Identifiers;
using Backend.Utils.Mappers;
using Backend.Domain.Game;
using Backend.Contracts.Data;

using static System.Math;

namespace Backend.Rooms.States;

public class PlayingGameState : GameState
{
    private const double TickRate = 24;
    private const double PlayerDistance = 2048;
    private const double IslandMargin = 128;

    private long _lastUpdate;
    private readonly double _radius;
    private readonly Dictionary<EID, Entity> _entities;

    public PlayingGameState(GameRoom gameRoom) : base(gameRoom)
    {
        _lastUpdate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        var playerCount = _gameRoom.Players.Count();
        var step = 2 * PI / playerCount;
        _radius = Floor(
            playerCount == 1
                ? PlayerDistance / 2
                : PlayerDistance / Sqrt(2 - 2 * Cos(2 * PI / playerCount))
        ) + IslandMargin;

        _entities = _gameRoom.Players.Select((player, i) => (Entity)new Tank(
            world: this,
            playerData: player,
            pos: new Vector(
                Sin(step * i) * (_radius - IslandMargin),
                0,
                Cos(step * i) * (_radius - IslandMargin)),
            pitch: step * i + PI
        )).ToDictionary(t => t.Eid);
    }

    public override GamePlayingStateDto RoomState => new()
    {
        Radius = _radius,
        Entities = _entities.Values.Select(e => e.ToDto()).ToList(),
        Scoreboard = Array.Empty<ScoreboardEntryDto>()
    };
}
