using Backend.Identifiers;

namespace Backend.Domain;

public interface IReadOnlyScoreboard
{
    public IEnumerable<CID> Players { get; }
    public IEnumerable<Scoreboard.Entry> Entries { get; }
}

public class Scoreboard : IReadOnlyScoreboard
{
    private readonly Dictionary<CID, int> _scores;
    private readonly IReadOnlyDictionary<CID, PlayerData> _metadata;

    public Scoreboard(IEnumerable<PlayerData> players)
    {
        _scores = players.ToDictionary(p => p.Cid, _ => 0);
        _metadata = players.ToDictionary(p => p.Cid, p => p);
    }

    public IEnumerable<CID> Players => _metadata.Keys;

    public IEnumerable<Entry> Entries => _scores
        .OrderByDescending(s => s.Value)
        .Select(s => new Entry
        {
            Cid = s.Key,
            PlayerData = _metadata[s.Key],
            Score = s.Value
        });

    public void Increment(CID cid)
    {
        _scores[cid]++;
    }

    public record Entry
    {
        public CID Cid { get; init; } = default!;
        public PlayerData PlayerData { get; init; } = default!;
        public int Score { get; init; } = default!;
    }
}
