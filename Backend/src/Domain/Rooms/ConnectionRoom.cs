using Backend.Contracts.Messages;
using Backend.Domain.Identifiers;
using Backend.Mediation.Requests;
using MediatR;

namespace Backend.Domain.Rooms;

internal abstract class ConnectionRoom
{
    protected readonly IMediator Mediator;
    protected readonly HashSet<Cid> PlayerIds;

    protected ConnectionRoom(IMediator mediator, HashSet<Cid> playerIds)
    {
        Mediator = mediator;
        PlayerIds = playerIds;
    }

    public IEnumerable<PlayerData> AllPlayers
        => PlayerIds.Select(cid => Mediator.Send(new PlayerDataRequest(cid)).Result);

    public bool HasPlayer(Cid cid) => PlayerIds.Contains(cid);

    public virtual async Task HandleOnJoinAsync(Cid cid)
    {
        PlayerIds.Add(cid);
        await Mediator.Send(new SendRoomStateRequest(cid, (this as GameRoom)?.Lid));
    }

    public virtual Task HandleOnLeaveAsync(Cid cid)
    {
        PlayerIds.Remove(cid);
        return Task.CompletedTask;
    }

    public virtual Task HandleOnMessageAsync(Cid cid, IClientMessage message) => Task.CompletedTask;
}
