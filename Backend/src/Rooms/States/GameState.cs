using Backend.Domain.Identifiers;
using Backend.Contracts.Messages;
using Backend.Contracts.Data;

namespace Backend.Rooms.States;

public abstract class GameState
{
    protected readonly GameRoom _gameRoom;

    protected GameState(GameRoom gameRoom) => _gameRoom = gameRoom;

    public abstract AbstractRoomStateDto RoomState { get; }

    public virtual Task HandleOnJoinAsync(CID cid) => Task.CompletedTask;

    public virtual Task HandleOnLeaveAsync(CID cid) => Task.CompletedTask;

    public virtual Task HandleOnMessageAsync(CID cid, IClientMessage message) => Task.CompletedTask;
}
