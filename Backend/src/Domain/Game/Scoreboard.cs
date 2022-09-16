using Backend.Domain.Identifiers;

namespace Backend.Domain.Game;

internal interface IReadOnlyScoreboard
{
    public IEnumerable<CID> Players { get; }
    public IEnumerable<Scoreboard.Entry> Entries { get; }
}

internal sealed class Scoreboard : IReadOnlyScoreboard
{
    private readonly Dictionary<CID, int> _scores;
    private readonly IReadOnlyDictionary<CID, PlayerData> _metadata;

    public Scoreboard(IEnumerable<PlayerData> players)
    {
        _scores = players.ToDictionary(p => p.CID, _ => 0);
        _metadata = players.ToDictionary(p => p.CID, p => p);
    }

    public IEnumerable<CID> Players => _metadata.Keys;

    public IEnumerable<Entry> Entries => _scores
        .OrderByDescending(s => s.Value)
        .Select(s => new Entry
        {
            CID = s.Key,
            PlayerData = _metadata[s.Key],
            Score = s.Value
        });

    public void Increment(CID cid)
    {
        _scores[cid]++;
    }

    public record Entry
    {
        public CID CID { get; init; } = default!;
        public PlayerData PlayerData { get; init; } = default!;
        public int Score { get; init; } = default!;
    }
}
