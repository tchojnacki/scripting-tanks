using Backend.Contracts.Messages;
using Backend.Domain.Identifiers;
using Backend.Domain.Rooms;

namespace Backend.Services;

public interface IRoomManager
{
    Task HandleOnConnectAsync(CID cid);
    Task HandleOnDisconnectAsync(CID cid);
    Task HandleOnMessageAsync(CID cid, IClientMessage message);

    // TODO
    bool CanPlayerCustomize(CID cid);
    IEnumerable<GameRoom> Lobbies { get; }
    Task UpsertLobbyAsync(GameRoom gameRoom);
    Task CloseLobbyAsync(GameRoom gameRoom);
    Task KickPlayerAsync(CID cid);
    Task JoinGameRoomAsync(CID cid, LID lid);
}
