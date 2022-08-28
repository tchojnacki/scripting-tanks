using System.Net.WebSockets;

namespace Backend.Domain;

public class ConnectionData
{
    public WebSocket Socket { get; init; } = default!;
    public string DisplayName { get; set; } = default!;
    public TankColors Colors { get; set; } = default!;
}
