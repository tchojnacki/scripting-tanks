using Backend.Contracts.Data;
using Backend.Rooms;

namespace Backend.Utils.Mappings;

public static class GameRoomMapper
{
    public static LobbyDto ToDto(this GameRoom room) => new()
    {
        LID = room.LID.ToString(),
        Name = room.Name,
        Players = room.Players.Count(),
        Location = room.Location,
    };
}
