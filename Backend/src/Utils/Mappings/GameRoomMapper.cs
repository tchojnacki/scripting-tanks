using Backend.Contracts.Data;
using Backend.Domain.Rooms;

namespace Backend.Utils.Mappings;

public static class GameRoomMapper
{
    public static LobbyDto ToDto(this GameRoom room) => new()
    {
        LID = room.LID.ToString(),
        Name = room.Name,
        Players = room.AllPlayers.Count(),
        Location = room.Location,
    };
}
