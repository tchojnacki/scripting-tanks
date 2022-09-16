using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Domain.Rooms.GameStates;

public class WaitingGameState : GameRoom
{
    protected override string Location => "game-waiting";

    private WaitingGameState(IMediator mediator, CID owner, LID lid, string name)
        : base(mediator, new(), owner, lid, name) { }

    private WaitingGameState(SummaryGameState previous) : base(previous) { }

    public static WaitingGameState CreateNew(IMediator mediator, CID owner, LID lid, string name)
        => new(mediator, owner, lid, name);

    public static WaitingGameState AfterSummary(SummaryGameState previous) => new(previous);
}
