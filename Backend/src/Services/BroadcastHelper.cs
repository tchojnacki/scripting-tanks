using Backend.Domain.Identifiers;
using Backend.Domain.Rooms;
using Backend.Contracts.Messages;

namespace Backend.Services;

public class BroadcastHelper : IBroadcastHelper
{
    private readonly IConnectionManager _connectionManager;
    private readonly IRoomManager _roomManager;

    public BroadcastHelper(IConnectionManager connectionManager, IRoomManager roomManager)
    {
        _connectionManager = connectionManager;
        _roomManager = roomManager;
    }

    private Task Broadcast<T>(ConnectionRoom connectionRoom, IServerMessage<T> message)
        => Task.WhenAll(
            connectionRoom.AllPlayers.Select(player => _connectionManager.SendToSingleAsync(
                player.CID,
                message)));

    public Task BroadcastToRoom<T>(LID lid, IServerMessage<T> message)
        => Broadcast(_roomManager.GetGameRoom(lid), message);

    public Task BroadcastToMenu<T>(IServerMessage<T> message)
        => Broadcast(_roomManager.MenuRoom, message);
}
