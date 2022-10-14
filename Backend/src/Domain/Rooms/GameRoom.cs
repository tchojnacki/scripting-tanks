using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Client;
using Backend.Domain.Identifiers;
using Backend.Mediation.Requests;
using MediatR;

namespace Backend.Domain.Rooms;

internal abstract class GameRoom : ConnectionRoom
{
    public const int MaxPlayers = 16;

    protected GameRoom(
        IMediator mediator,
        HashSet<Cid> playerIds,
        Cid owner,
        Lid lid,
        string name)
        : base(mediator, playerIds)
    {
        Lid = lid;
        OwnerCid = owner;
        Name = name;
    }

    protected GameRoom(GameRoom previous)
        : this(previous.Mediator, previous.PlayerIds, previous.OwnerCid, previous.Lid, previous.Name)
    {
    }

    public Lid Lid { get; }
    public string Name { get; }
    public Cid OwnerCid { get; private set; }
    protected abstract string Location { get; }

    public LobbyInfo LobbyInfo => new()
    {
        Lid = Lid,
        Name = Name,
        PlayerCount = AllPlayers.Count(),
        Location = Location
    };

    private async Task PromoteAsync(Cid cid)
    {
        OwnerCid = cid;
        await Mediator.Send(new BroadcastOwnerChangeRequest(Lid, OwnerCid));
    }

    public override async Task HandleOnJoinAsync(Cid cid)
    {
        await base.HandleOnJoinAsync(cid);
        await Mediator.Send(new BroadcastNewPlayerRequest(Lid, cid));
        await Mediator.Send(new BroadcastUpsertLobbyRequest(Lid));
    }

    public override async Task HandleOnLeaveAsync(Cid cid)
    {
        await base.HandleOnLeaveAsync(cid);

        var realPlayers = AllPlayers.Where(p => !p.IsBot).Select(p => p.Cid).ToList();
        if (realPlayers.Any())
        {
            if (cid == OwnerCid)
            {
                var random = new Random();
                OwnerCid = realPlayers.ElementAt(random.Next(realPlayers.Count));
                await Mediator.Send(new BroadcastOwnerChangeRequest(Lid, OwnerCid));
            }

            await Mediator.Send(new BroadcastPlayerLeftRequest(Lid, cid));
            await Mediator.Send(new BroadcastUpsertLobbyRequest(Lid));
        }
        else
        {
            await Mediator.Send(new CloseLobbyRequest(Lid));
        }
    }

    public override Task HandleOnMessageAsync(Cid cid, IClientMessage message) => message switch
    {
        PromotePlayerClientMessage { Data: var target } => PromoteAsync(Cid.Deserialize(target)),
        _ => base.HandleOnMessageAsync(cid, message)
    };
}