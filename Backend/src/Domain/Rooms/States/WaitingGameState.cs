using MediatR;
using Backend.Utils.Mappings;
using Backend.Domain.Identifiers;
using Backend.Contracts.Data;

namespace Backend.Domain.Rooms.States;

public class WaitingGameState : GameRoom
{
    private WaitingGameState(IMediator mediator, CID owner, LID lid, string name)
        : base(mediator, new(), owner, lid, name) { }

    private WaitingGameState(SummaryGameState previous) : base(previous) { }

    public static WaitingGameState CreateNew(IMediator mediator, CID owner, LID lid, string name)
        => new(mediator, owner, lid, name);

    public static WaitingGameState AfterSummary(SummaryGameState previous) => new(previous);

    public override GameWaitingStateDto RoomState => new()
    {
        Name = Name,
        Owner = OwnerCID.ToString(),
        Players = AllPlayers.Select(p => p.ToDto()).ToList(),
    };
}
