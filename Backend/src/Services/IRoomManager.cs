using Backend.Contracts.Messages;
using Backend.Domain.Identifiers;
using Backend.Domain.Rooms;

namespace Backend.Services;

internal interface IRoomManager
{
    MenuRoom MenuRoom { get; }
    IEnumerable<LobbyInfo> Lobbies { get; }
    Task HandleOnConnectAsync(Cid cid);
    Task HandleOnDisconnectAsync(Cid cid);
    Task HandleOnMessageAsync(Cid cid, IClientMessage message);
    GameRoom GetGameRoom(Lid lid);
    bool ContainsGameRoom(Lid lid);
    ConnectionRoom RoomContaining(Cid cid);
    Task JoinGameRoomAsync(Cid cid, Lid lid);
    Task CloseLobbyAsync(Lid lid);
    Task ShowSummaryAsync(Lid lid);
    Task PlayAgainAsync(Lid lid);
}