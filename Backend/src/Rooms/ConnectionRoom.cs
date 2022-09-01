using Backend.Identifiers;
using Backend.Services;
using Backend.Contracts.Data;
using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Server;

namespace Backend.Rooms;

public abstract class ConnectionRoom
{
    protected readonly HashSet<CID> _playerIds = new();

    protected readonly IConnectionManager _connectionManager;
    protected readonly RoomManager _roomManager;

    protected ConnectionRoom(IConnectionManager connectionManager, RoomManager roomManager)
    {
        _connectionManager = connectionManager;
        _roomManager = roomManager;
    }

    public abstract AbstractRoomStateDto RoomState { get; }

    public bool HasPlayer(CID cid) => _playerIds.Contains(cid);

    public virtual async Task HandleOnJoinAsync(CID cid)
    {
        _playerIds.Add(cid);
        await _connectionManager.SendToSingleAsync(cid, new RoomStateServerMessage { Data = RoomState });
    }

    public virtual Task HandleOnLeaveAsync(CID cid)
    {
        _playerIds.Remove(cid);
        return Task.CompletedTask;
    }

    public Task BroadcastMessageAsync<T>(IServerMessage<T> message)
        => Task.WhenAll(_playerIds.Select(cid => _connectionManager.SendToSingleAsync(cid, message)));
}
