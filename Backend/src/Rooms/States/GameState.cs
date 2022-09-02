using Backend.Identifiers;
using Backend.Contracts.Messages;
using Backend.Contracts.Data;

namespace Backend.Rooms.States;

public abstract class GameState
{
    protected readonly GameRoom _gameRoom;

    protected GameState(GameRoom gameRoom) => _gameRoom = gameRoom;

    public abstract AbstractGameStateDto RoomState { get; }

    public virtual Task HandleOnJoinAsync(CID cid) => Task.CompletedTask;

    public virtual Task HandleOnLeaveAsync(CID cid) => Task.CompletedTask;

    public virtual Task HandleOnMessageAsync(CID cid, IClientMessage<object?> message) => Task.CompletedTask;
}