using Backend.Contracts.Messages;
using Backend.Domain.Identifiers;
using Backend.Domain.Rooms;

namespace Backend.Services;

public interface IRoomManager
{
    Task HandleOnConnectAsync(CID cid);
    Task HandleOnDisconnectAsync(CID cid);
    Task HandleOnMessageAsync(CID cid, IClientMessage message);
    GameRoom GetRoom(LID lid);
    MenuRoom MenuRoom { get; }
    IEnumerable<GameRoom> Lobbies { get; }
    Task KickPlayerAsync(CID cid);
    Task JoinGameRoomAsync(CID cid, LID lid);
    Task CloseLobbyAsync(LID lid);

    // TODO
    bool CanPlayerCustomize(CID cid);
}
