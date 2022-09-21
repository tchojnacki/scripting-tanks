using Backend.Domain.Game;
using Backend.Domain.Game.Controls;
using Backend.Mediation.Requests;

using static System.Math;

namespace Backend.Domain.Rooms.GameStates;

internal sealed class SummaryGameState : GameRoom
{
    private const int SummaryDuration = 5;
    private const double PodiumRadius = 5;
    private const double PodiumHeight = 0.25;

    protected override string Location => "game-summary";

    private SummaryGameState(PlayingGameState previous) : base(previous)
    {
        Scoreboard = previous.Scoreboard;
        Task.Run(async () => await WaitToPlayAgain());
    }

    public static SummaryGameState AfterPlaying(PlayingGameState previous) => new(previous);

    public IReadOnlyScoreboard Scoreboard { get; }

    public int Remaining { get; private set; } = SummaryDuration;

    public IEnumerable<Tank> Tanks => Scoreboard.Entries.Take(3).Select((entry, index) =>
    {
        var angle = index * PI / 6 + 5 * PI / 6;
        return new Tank(
            default!,
            entry.PlayerData,
            new(PodiumRadius * Sin(angle), PodiumHeight * (3 - index), PodiumRadius * Cos(angle)),
            PI + angle,
            new IdleTankController());
    });

    private async Task WaitToPlayAgain()
    {
        while (Remaining > 0)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            Remaining--;
            await _mediator.Send(new BroadcastRoomStateRequest(LID));
        }
        await _mediator.Send(new PlayAgainRequest(LID));
    }
}