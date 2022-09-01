using Backend.Contracts.Data;
using Backend.Rooms;

namespace Backend.Utils.Mappers;

public static class GameRoomMapper
{
    public static LobbyDto ToDto(this GameRoom model) => new()
    {
        Lid = model.Lid.Value,
        Name = model.Name,
        Players = model.Players.Count(),
        Location = model.Location,
    };
}
