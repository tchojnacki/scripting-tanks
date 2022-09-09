using Backend.Identifiers;
using Backend.Contracts.Data;
using Backend.Contracts.Messages.Server;

namespace Backend.Rooms.States;

public class SummaryGameState : GameState
{
    private const int SummaryDuration = 10;

    private readonly Dictionary<CID, int> _scoreboard;
    private int _remaining = SummaryDuration;

    public SummaryGameState(GameRoom room, Dictionary<CID, int> scoreboard) : base(room)
    {
        _scoreboard = scoreboard;
        Task.Run(async () => await WaitToPlayAgain());
    }

    private async Task WaitToPlayAgain()
    {
        while (_remaining > 0)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            _remaining--;
            await _gameRoom.BroadcastMessageAsync(new RoomStateServerMessage { Data = RoomState });
        }
        await _gameRoom.PlayAgain();
    }

    public override GameSummaryStateDto RoomState => new()
    {
        Remaining = _remaining,
        Scoreboard = Array.Empty<ScoreboardEntryDto>(),
        Tanks = Array.Empty<TankDto>()
    };
}
