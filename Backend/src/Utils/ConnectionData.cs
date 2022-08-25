using System.Net.WebSockets;
using Backend.Dtos;

namespace Backend.Utils;

public readonly record struct ConnectionData
{
    public WebSocket Socket { get; init; }
    public string DisplayName { get; init; }
}
