using Backend.Contracts.Data;
using Backend.Rooms;

namespace Backend.Utils.Mappers;

public static class GameRoomMapper
{
    public static LobbyDto ToDto(this GameRoom room) => new()
    {
        Lid = room.Lid.Value,
        Name = room.Name,
        Players = room.Players.Count(),
        Location = room.Location,
    };
}
