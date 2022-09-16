using MediatR;
using Backend.Domain.Identifiers;
using Backend.Domain.Rooms.States;
using Backend.Contracts.Data;
using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Client;
using Backend.Mediation.Requests;

namespace Backend.Domain.Rooms;

public abstract class GameRoom : ConnectionRoom
{
    private static readonly Random Rand = new();

    protected GameRoom(
        IMediator mediator,
        HashSet<CID> playerIds,
        CID owner,
        LID lid,
        string name)
        : base(mediator, playerIds)
    {
        LID = lid;
        OwnerCID = owner;
        Name = name;
    }

    protected GameRoom(GameRoom previous)
        : this(previous._mediator, previous._playerIds, previous.OwnerCID, previous.LID, previous.Name) { }

    public LID LID { get; }
    public string Name { get; }
    public CID OwnerCID { get; private set; }

    public string Location => RoomState.Location;

    private async Task PromoteAsync(CID cid)
    {
        OwnerCID = cid;
        await _mediator.Send(new BroadcastOwnerChangeRequest(LID, OwnerCID));
    }

    public override async Task HandleOnJoinAsync(CID cid)
    {
        await base.HandleOnJoinAsync(cid);
        await _mediator.Send(new BroadcastUpsertLobbyRequest(LID));
    }

    public override async Task HandleOnLeaveAsync(CID cid)
    {
        await base.HandleOnLeaveAsync(cid);
        if (cid == OwnerCID)
        {
            var ownerCandidates = RealPlayers.Select(p => p.CID).ToList();
            if (ownerCandidates.Any())
            {
                OwnerCID = ownerCandidates.ElementAt(Rand.Next(ownerCandidates.Count));
                await _mediator.Send(new BroadcastOwnerChangeRequest(LID, OwnerCID));
            }
            else
            {
                await _mediator.Send(new CloseLobbyRequest(LID));
            }
        }

        if (RealPlayers.Any())
            await _mediator.Send(new BroadcastUpsertLobbyRequest(LID));
    }

    public override Task HandleOnMessageAsync(CID cid, IClientMessage message) => message switch
    {
        PromotePlayerClientMessage { Data: var target } => PromoteAsync(CID.Deserialize(target)),
        _ => Task.CompletedTask
    };
}
