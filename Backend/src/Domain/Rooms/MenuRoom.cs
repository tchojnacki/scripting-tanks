using Backend.Services;
using Backend.Contracts.Data;
using Backend.Utils.Mappings;

namespace Backend.Domain.Rooms;

public class MenuRoom : ConnectionRoom
{
    public MenuRoom(Func<IConnectionManager> connectionManager, IRoomManager roomManager)
        : base(connectionManager, roomManager) { }

    public override MenuStateDto RoomState => new()
    {
        Lobbies = _roomManager.Lobbies.Select(l => l.ToDto()).ToList()
    };
}
