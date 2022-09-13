using MediatR;
using Backend.Domain.Identifiers;
using Backend.Contracts.Messages;
using Backend.Contracts.Data;

namespace Backend.Domain.Rooms.States;

public abstract class GameState
{
    protected readonly IMediator _mediator;
    protected readonly GameRoom _gameRoom;

    protected GameState(IMediator mediator, GameRoom gameRoom)
    {
        _mediator = mediator;
        _gameRoom = gameRoom;
    }

    public abstract AbstractRoomStateDto RoomState { get; }

    public virtual Task HandleOnJoinAsync(CID cid) => Task.CompletedTask;

    public virtual Task HandleOnLeaveAsync(CID cid) => Task.CompletedTask;

    public virtual Task HandleOnMessageAsync(CID cid, IClientMessage message) => Task.CompletedTask;
}
