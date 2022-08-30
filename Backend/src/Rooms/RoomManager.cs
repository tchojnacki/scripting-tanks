using Backend.Services;
using Backend.Identifiers;
using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Client;

namespace Backend.Rooms;

public class RoomManager
{
    private readonly IConnectionManager _connectionManager;

    public RoomManager(IConnectionManager connectionManager) => _connectionManager = connectionManager;

    private Task CreateLobbyAsync(CID cid) => Task.CompletedTask;

    private Task JoinGameRoomAsync(CID cid, LID lid) => Task.CompletedTask;

    private Task KickPlayerAsync(CID cid) => Task.CompletedTask;

    public bool PlayerCanCustomize(CID cid) => true;

    public Task HandleOnConnectAsync(CID cid) => Task.CompletedTask;

    public Task HandleOnDisconnectAsync(CID cid) => Task.CompletedTask;

    public async Task HandleOnMessageAsync(CID cid, IClientMessage<object?> message)
    {
        switch (message)
        {
            case CreateLobbyClientMessage:
                await CreateLobbyAsync(cid);
                break;

            case EnterLobbyClientMessage { Data: var lidString }:
                await JoinGameRoomAsync(cid, LID.From(lidString));
                break;

            case LeaveLobbyClientMessage:
                await KickPlayerAsync(cid);
                break;

            default:
                break;
        }
    }
}
