using Backend.Domain.Identifiers;

namespace Backend.Domain.Game;

internal interface IReadOnlyScoreboard
{
    public IEnumerable<Cid> Players { get; }
    public IEnumerable<Scoreboard.Entry> Entries { get; }
}

internal sealed class Scoreboard : IReadOnlyScoreboard
{
    private readonly IReadOnlyDictionary<Cid, PlayerData> _metadata;
    private readonly Dictionary<Cid, int> _scores;

    public Scoreboard(IEnumerable<PlayerData> players)
    {
        var playerList = players.ToList();
        _scores = playerList.ToDictionary(p => p.Cid, _ => 0);
        _metadata = playerList.ToDictionary(p => p.Cid, p => p);
    }

    public IEnumerable<Cid> Players => _metadata.Keys;

    public IEnumerable<Entry> Entries => _scores
        .OrderByDescending(s => s.Value)
        .Select(s => new Entry
        {
            Cid = s.Key,
            PlayerData = _metadata[s.Key],
            Score = s.Value
        });

    public void Increment(Cid cid)
    {
        _scores[cid]++;
    }

    public record Entry
    {
        public Cid Cid { get; init; } = default!;
        public PlayerData PlayerData { get; init; } = default!;
        public int Score { get; init; }
    }
}
