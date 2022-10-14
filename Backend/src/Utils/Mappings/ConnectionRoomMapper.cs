using Backend.Contracts.Data;
using Backend.Domain.Rooms;
using Backend.Domain.Rooms.GameStates;

namespace Backend.Utils.Mappings;

internal static class ConnectionRoomMapper
{
    public static AbstractRoomStateDto ToDto(this ConnectionRoom room) => room switch
    {
        PlayingGameState state => state.ToDto(),
        SummaryGameState state => state.ToDto(),
        WaitingGameState state => state.ToDto(),
        MenuRoom state => state.ToDto(),
        _ => throw new NotImplementedException()
    };

    public static MenuStateDto ToDto(this MenuRoom room) => new()
    {
        Lobbies = room.Lobbies.Select(l => l.ToDto()).ToList()
    };

    public static GamePlayingStateDto ToDto(this PlayingGameState state) => new()
    {
        Radius = state.Radius,
        Entities = state.Entities.Select(e => e.ToDto()).ToList(),
        Scoreboard = state.Scoreboard.ToDto()
    };

    public static GameSummaryStateDto ToDto(this SummaryGameState state) => new()
    {
        Remaining = state.Remaining,
        Scoreboard = state.Scoreboard.ToDto(),
        Tanks = state.Tanks.Select(t => t.ToDto()).ToList()
    };

    public static GameWaitingStateDto ToDto(this WaitingGameState state) => new()
    {
        Name = state.Name,
        Owner = state.OwnerCid.ToString(),
        Players = state.AllPlayers.Select(p => p.ToDto()).ToList()
    };
}