using MediatR;
using Backend.Domain.Identifiers;
using Backend.Contracts.Data;
using Backend.Contracts.Messages;
using Backend.Mediation.Requests;

namespace Backend.Domain.Rooms;

public abstract class ConnectionRoom
{
    protected readonly HashSet<CID> _playerIds = new();

    protected readonly IMediator _mediator;

    protected ConnectionRoom(IMediator mediator) => _mediator = mediator;

    public abstract AbstractRoomStateDto RoomState { get; }

    public IEnumerable<PlayerData> AllPlayers
        => _playerIds.Select(cid => _mediator.Send(new PlayerDataRequest(cid)).Result);

    public IEnumerable<PlayerData> RealPlayers
        => AllPlayers.Where(p => !p.IsBot);

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
