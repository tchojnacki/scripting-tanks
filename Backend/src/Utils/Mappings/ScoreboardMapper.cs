using Backend.Contracts.Data;
using Backend.Domain;

namespace Backend.Utils.Mappings;

public static class ScoreboardMapper
{
    public static ScoreboardEntryDto ToDto(this Scoreboard.Entry entry) => new()
    {
        Cid = entry.Cid.Value,
        Name = entry.Name,
        Score = entry.Score
    };

    public static IReadOnlyList<ScoreboardEntryDto> ToDto(this IReadOnlyScoreboard scoreboard)
        => scoreboard.Entries.Select(e => e.ToDto()).ToList();
}
