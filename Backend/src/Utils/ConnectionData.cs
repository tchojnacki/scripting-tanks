using System.Net.WebSockets;

namespace Backend.Utils;

public readonly record struct ConnectionData
{
    public WebSocket Socket { get; init; }
    public string DisplayName { get; init; }
    public IReadOnlyList<string> Colors { get; init; }
}
