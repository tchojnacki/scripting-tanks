using Backend.Services;
using Backend.Contracts.Data;
using Backend.Utils.Mappers;

namespace Backend.Rooms;

public class MenuRoom : ConnectionRoom
{
    public MenuRoom(IConnectionManager connectionManager, RoomManager roomManager)
        : base(connectionManager, roomManager) { }

    public override MenuStateDto RoomState => new()
    {
        Lobbies = _roomManager.Lobbies.Select(l => l.ToDto()).ToList()
    };
}
