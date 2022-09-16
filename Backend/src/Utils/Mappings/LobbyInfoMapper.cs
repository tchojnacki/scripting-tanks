using Backend.Contracts.Data;
using Backend.Domain.Rooms;

namespace Backend.Utils.Mappings;

public static class LobbyInfoMapper
{
    public static LobbyDto ToDto(this LobbyInfo lobby) => new()
    {
        LID = lobby.LID.ToString(),
        Name = lobby.Name,
        Players = lobby.PlayerCount,
        Location = lobby.Location,
    };
}
