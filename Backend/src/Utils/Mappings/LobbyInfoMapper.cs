using Backend.Contracts.Data;
using Backend.Domain.Rooms;

namespace Backend.Utils.Mappings;

internal static class LobbyInfoMapper
{
    public static LobbyDto ToDto(this LobbyInfo lobby) => new()
    {
        Lid = lobby.Lid.ToString(),
        Name = lobby.Name,
        Players = lobby.PlayerCount,
        Location = lobby.Location
    };
}