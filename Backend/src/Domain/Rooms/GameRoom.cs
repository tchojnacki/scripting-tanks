using MediatR;
using Backend.Domain.Identifiers;
using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Client;
using Backend.Mediation.Requests;

namespace Backend.Domain.Rooms;

internal abstract class GameRoom : ConnectionRoom
{
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
    protected abstract string Location { get; }

    public LobbyInfo LobbyInfo => new()
    {
        LID = LID,
        Name = Name,
        PlayerCount = AllPlayers.Count(),
        Location = Location,
    };

    private async Task PromoteAsync(CID cid)
    {
        OwnerCID = cid;
        await _mediator.Send(new BroadcastOwnerChangeRequest(LID, OwnerCID));
    }

    public override async Task HandleOnJoinAsync(CID cid)
    {
        await base.HandleOnJoinAsync(cid);
        await _mediator.Send(new BroadcastNewPlayerRequest(LID, cid));
        await _mediator.Send(new BroadcastUpsertLobbyRequest(LID));
    }

    public override async Task HandleOnLeaveAsync(CID cid)
    {
        await base.HandleOnLeaveAsync(cid);

        var realPlayers = AllPlayers.Where(p => !p.IsBot).Select(p => p.CID).ToList();
        if (realPlayers.Any())
        {
            if (cid == OwnerCID)
            {
                var random = new Random();
                OwnerCID = realPlayers.ElementAt(random.Next(realPlayers.Count));
                await _mediator.Send(new BroadcastOwnerChangeRequest(LID, OwnerCID));
            }

            await _mediator.Send(new BroadcastPlayerLeftRequest(LID, cid));
            await _mediator.Send(new BroadcastUpsertLobbyRequest(LID));
        }
        else
        {
            await _mediator.Send(new CloseLobbyRequest(LID));
        }
    }

    public override Task HandleOnMessageAsync(CID cid, IClientMessage message) => message switch
    {
        PromotePlayerClientMessage { Data: var target } => PromoteAsync(CID.Deserialize(target)),
        _ => base.HandleOnMessageAsync(cid, message)
    };
}
