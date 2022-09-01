using Backend.Contracts.Data;
using Backend.Services;

namespace Backend.Rooms;

public class MenuRoom : ConnectionRoom
{
    public MenuRoom(IConnectionManager connectionManager, RoomManager roomManager)
        : base(connectionManager, roomManager) { }

    public override AbstractRoomStateDto RoomState => new MenuStateDto
    {
        Lobbies = _roomManager.LobbyEntries
    };
}
