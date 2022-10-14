using Backend.Contracts.Data;
using Backend.Domain.Game;

namespace Backend.Utils.Mappings;

internal static class ScoreboardMapper
{
    public static ScoreboardEntryDto ToDto(this Scoreboard.Entry entry) => new()
    {
        Cid = entry.Cid.ToString(),
        Name = entry.PlayerData.Name,
        Score = entry.Score
    };

    public static IReadOnlyList<ScoreboardEntryDto> ToDto(this IReadOnlyScoreboard scoreboard)
        => scoreboard.Entries.Select(e => e.ToDto()).ToList();
}