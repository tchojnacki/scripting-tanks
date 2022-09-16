using MediatR;
using Backend.Domain.Identifiers;
using Backend.Contracts.Messages;
using Backend.Mediation.Requests;

namespace Backend.Domain.Rooms;

public abstract class ConnectionRoom
{
    protected readonly IMediator _mediator;
    protected readonly HashSet<CID> _playerIds;

    protected ConnectionRoom(IMediator mediator, HashSet<CID> playerIds)
    {
        _mediator = mediator;
        _playerIds = playerIds;
    }

    public IEnumerable<PlayerData> AllPlayers
        => _playerIds.Select(cid => _mediator.Send(new PlayerDataRequest(cid)).Result);

    public bool HasPlayer(CID cid) => _playerIds.Contains(cid);

    public virtual async Task HandleOnJoinAsync(CID cid)
    {
        _playerIds.Add(cid);
        await _mediator.Send(new SendRoomStateRequest(cid, (this as GameRoom)?.LID));
    }

    public virtual Task HandleOnLeaveAsync(CID cid)
    {
        _playerIds.Remove(cid);
        return Task.CompletedTask;
    }

    public virtual Task HandleOnMessageAsync(CID cid, IClientMessage message) => Task.CompletedTask;
}
