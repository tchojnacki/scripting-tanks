using Backend.Domain.Identifiers;
using MediatR;

namespace Backend.Domain.Rooms.GameStates;

internal sealed class WaitingGameState : GameRoom
{
    private WaitingGameState(IMediator mediator, Cid owner, Lid lid, string name)
        : base(mediator, new(), owner, lid, name)
    {
    }

    private WaitingGameState(SummaryGameState previous) : base(previous)
    {
    }

    protected override string Location => "game-waiting";

    public static WaitingGameState CreateNew(IMediator mediator, Cid owner, Lid lid, string name)
        => new(mediator, owner, lid, name);

    public static WaitingGameState AfterSummary(SummaryGameState previous) => new(previous);
}