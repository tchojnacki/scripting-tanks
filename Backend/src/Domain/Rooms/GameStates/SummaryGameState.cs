using Backend.Domain.Identifiers;
using Backend.Domain.Game;
using Backend.Contracts.Data;
using Backend.Utils.Mappings;
using Backend.Mediation.Requests;

using static System.Math;

namespace Backend.Domain.Rooms.GameStates;

public class SummaryGameState : GameRoom
{
    private const int SummaryDuration = 10;
    private const double PodiumRadius = 512;
    private const double PodiumHeight = 32;

    private readonly IReadOnlyScoreboard _scoreboard;
    private int _remaining = SummaryDuration;

    private SummaryGameState(PlayingGameState previous) : base(previous)
    {
        _scoreboard = previous.Scoreboard;
        Task.Run(async () => await WaitToPlayAgain());
    }

    public static SummaryGameState AfterPlaying(PlayingGameState previous) => new(previous);

    private async Task WaitToPlayAgain()
    {
        while (_remaining > 0)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            _remaining--;
            await _mediator.Send(new BroadcastRoomStateRequest(LID));
        }
        await _mediator.Send(new PlayAgainRequest(LID));
    }

    public override GameSummaryStateDto RoomState => new()
    {
        Remaining = _remaining,
        Scoreboard = _scoreboard.ToDto(),
        Tanks = _scoreboard.Entries.Take(3).Select((entry, index) =>
        {
            var angle = index * PI / 6 + 5 * PI / 6;
            return new TankDto
            {
                CID = entry.CID.ToString(),
                EID = EID.FromCID(entry.CID).ToString(),
                Name = entry.PlayerData.Name,
                Colors = entry.PlayerData.Colors.ToDto(),
                Pos = new()
                {
                    X = PodiumRadius * Sin(angle),
                    Y = PodiumHeight * (3 - index),
                    Z = PodiumRadius * Cos(angle)
                },
                Pitch = PI + angle,
                Barrel = PI + angle,
            };
        }).ToList()
    };
}
