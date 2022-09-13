using Backend.Domain.Rooms;
using Backend.Domain.Identifiers;
using Backend.Utils.Mappings;
using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Client;
using Backend.Contracts.Messages.Server;

namespace Backend.Services;

public class RoomManager : IRoomManager
{
    private readonly MenuRoom _menuRoom;
    private readonly Dictionary<LID, GameRoom> _gameRooms;

    private readonly Func<IConnectionManager> _connectionManager;

    public RoomManager(Func<IConnectionManager> connectionManager)
    {
        _connectionManager = connectionManager;

        _menuRoom = new MenuRoom(connectionManager, this);
        _gameRooms = new();
    }

    public IEnumerable<GameRoom> Lobbies => _gameRooms.Values;

    public async Task CloseLobbyAsync(GameRoom gameRoom)
    {
        if (_gameRooms.ContainsKey(gameRoom.LID))
        {
            _gameRooms.Remove(gameRoom.LID);
            await _menuRoom.BroadcastMessageAsync(new LobbyRemovedServerMessage { Data = gameRoom.LID.ToString() });
        }
        await Task.WhenAll(gameRoom.Players.Select(p => _menuRoom.HandleOnJoinAsync(p.CID)));
    }

    public bool CanPlayerCustomize(CID cid) => RoomContaining(cid) == _menuRoom;

    public Task HandleOnConnectAsync(CID cid) => SwitchRoomAsync(cid, _menuRoom);

    public Task HandleOnDisconnectAsync(CID cid) => SwitchRoomAsync(cid, null);

    public async Task HandleOnMessageAsync(CID cid, IClientMessage message)
    {
        var room = RoomContaining(cid)!;

        await (message switch
        {
            CreateLobbyClientMessage when room == _menuRoom
                => CreateLobbyAsync(cid),
            EnterLobbyClientMessage { Data: var lidString } when room == _menuRoom
                => JoinGameRoomAsync(cid, LID.Deserialize(lidString)),
            LeaveLobbyClientMessage when room != _menuRoom
                => KickPlayerAsync(cid),
            _ => room.HandleOnMessageAsync(cid, message)
        });
    }

    public Task JoinGameRoomAsync(CID cid, LID lid) => SwitchRoomAsync(cid, _gameRooms[lid]);

    public Task KickPlayerAsync(CID cid)
        => SwitchRoomAsync(cid, _connectionManager().DataFor(cid).IsBot ? null : _menuRoom);

    public Task UpsertLobbyAsync(GameRoom gameRoom) => _menuRoom.BroadcastMessageAsync(
        new UpsertLobbyServerMessage { Data = gameRoom.ToDto() });

    private ConnectionRoom? RoomContaining(CID cid)
        => new ConnectionRoom[] { _menuRoom }
            .Concat(_gameRooms.Values.AsEnumerable())
            .FirstOrDefault(r => r.HasPlayer(cid));

    private async Task SwitchRoomAsync(CID cid, ConnectionRoom? newRoom)
    {
        var previousRoom = RoomContaining(cid);

        if (previousRoom is not null)
        {
            await previousRoom.HandleOnLeaveAsync(cid);
            if (previousRoom is GameRoom gr && gr.RealPlayers.Any())
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
        var lid = LID.GenerateUnique();
        var name = $"{_connectionManager().DataFor(cid).Name}'s Game";
        _gameRooms[lid] = new(_connectionManager, this, cid, lid, name);
        await UpsertLobbyAsync(_gameRooms[lid]);
        await JoinGameRoomAsync(cid, lid);
    }
}
