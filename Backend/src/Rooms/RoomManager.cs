using Backend.Services;
using Backend.Identifiers;
using Backend.Contracts.Data;
using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Client;
using Backend.Contracts.Messages.Server;

namespace Backend.Rooms;

public class RoomManager
{
    private readonly MenuRoom _menuRoom;
    private readonly Dictionary<LID, GameRoom> _gameRooms;

    private readonly IConnectionManager _connectionManager;

    public RoomManager(IConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;

        _menuRoom = new MenuRoom(connectionManager, this);
        _gameRooms = new();
    }

    public IReadOnlyList<LobbyDto> LobbyEntries => _gameRooms.Values.Select(r => r.LobbyData).ToList();

    public async Task CloseLobbyAsync(GameRoom gameRoom)
    {
        if (_gameRooms.ContainsKey(gameRoom.Lid))
        {
            _gameRooms.Remove(gameRoom.Lid);
            await _menuRoom.BroadcastMessageAsync(new LobbyRemovedServerMessage { Data = gameRoom.Lid.Value });
        }
        await Task.WhenAll(gameRoom.Players.Select(p => _menuRoom.HandleOnJoinAsync(CID.From(p.Cid))));
    }

    private ConnectionRoom? RoomContaining(CID cid)
        => new ConnectionRoom[] { _menuRoom }
            .Concat(_gameRooms.Values.AsEnumerable())
            .FirstOrDefault(r => r.HasPlayer(cid));

    private Task UpsertLobbyAsync(GameRoom gameRoom) => _menuRoom.BroadcastMessageAsync(
        new UpsertLobbyServerMessage { Data = gameRoom.LobbyData }
    );

    private async Task SwitchRoomAsync(CID cid, ConnectionRoom? newRoom)
    {
        var previousRoom = RoomContaining(cid);

        if (previousRoom is not null)
        {
            await previousRoom.HandleOnLeaveAsync(cid);
            if (previousRoom is GameRoom gr && gr.RealPlayerCount > 0)
                await UpsertLobbyAsync(gr);
        }

        if (newRoom is not null)
        {
            await newRoom.HandleOnJoinAsync(cid);
            if (newRoom is GameRoom gr)
                await UpsertLobbyAsync(gr);
        }
    }

    private async Task CreateLobbyAsync(CID cid)
    {
        var lid = LID.From("LID$" + Guid.NewGuid().ToString());
        var name = $"{_connectionManager.PlayerData(cid).Name}'s Game";
        _gameRooms[lid] = new(_connectionManager, this, cid, lid, name);
        await UpsertLobbyAsync(_gameRooms[lid]);
        await JoinGameRoomAsync(cid, lid);
    }

    private Task JoinGameRoomAsync(CID cid, LID lid) => SwitchRoomAsync(cid, _gameRooms[lid]);

    private Task KickPlayerAsync(CID cid) => SwitchRoomAsync(cid, null);

    public bool CanPlayerCustomize(CID cid) => RoomContaining(cid) == _menuRoom;

    public Task HandleOnConnectAsync(CID cid) => SwitchRoomAsync(cid, _menuRoom);

    public Task HandleOnDisconnectAsync(CID cid) => SwitchRoomAsync(cid, null);

    public async Task HandleOnMessageAsync(CID cid, IClientMessage<object?> message)
    {
        var room = RoomContaining(cid);
        switch (message)
        {
            case CreateLobbyClientMessage when room == _menuRoom:
                await CreateLobbyAsync(cid);
                break;

            case EnterLobbyClientMessage { Data: var lidString } when room == _menuRoom:
                await JoinGameRoomAsync(cid, LID.From(lidString));
                break;

            case LeaveLobbyClientMessage when room != _menuRoom:
                await KickPlayerAsync(cid);
                break;

            default:
                break;
        }
    }
}
