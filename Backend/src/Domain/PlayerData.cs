using System.Net.WebSockets;
using Backend.Domain.Identifiers;

namespace Backend.Domain;

internal sealed class PlayerData
{
    public CID CID { get; init; } = default!;
    public WebSocket? Socket { get; init; } = default!;
    public string Name { get; set; } = default!;
    public TankColors Colors { get; set; } = default!;

    public bool IsBot => Socket == null;
}
