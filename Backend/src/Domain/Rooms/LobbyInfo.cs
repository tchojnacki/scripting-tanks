using Backend.Domain.Identifiers;

namespace Backend.Domain.Rooms;

internal sealed record LobbyInfo
{
    public LID LID { get; init; } = default!;
    public string Name { get; init; } = default!;
    public int PlayerCount { get; init; } = default!;
    public string Location { get; init; } = default!;
}
