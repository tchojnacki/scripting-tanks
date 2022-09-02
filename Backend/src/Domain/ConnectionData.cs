using System.Net.WebSockets;
using Backend.Identifiers;

namespace Backend.Domain;

public class ConnectionData
{
    public CID Cid { get; init; } = default!;
    public WebSocket? Socket { get; init; } = default!;
    public string DisplayName { get; set; } = default!;
    public TankColors Colors { get; set; } = default!;

    public bool IsBot => Socket == null;
}
