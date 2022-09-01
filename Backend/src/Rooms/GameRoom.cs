using Backend.Services;
using Backend.Identifiers;
using Backend.Domain;
using Backend.Contracts.Data;
using Backend.Contracts.Messages.Server;
using Backend.Rooms.States;

namespace Backend.Rooms;

public class GameRoom : ConnectionRoom
{
    private static readonly Random Rand = new();

    private readonly GameState _gameState;

    public GameRoom(
        IConnectionManager connectionManager,
        RoomManager roomManager,
        CID owner,
        LID lid,
        string name)
        : base(connectionManager, roomManager)
    {
        Lid = lid;
        Owner = owner;
        Name = name;
        _gameState = new WaitingGameState(this);
    }

    public LID Lid { get; }
    public string Name { get; }
    public CID Owner { get; private set; }

    public int RealPlayerCount => _playerIds.Count;
    public IEnumerable<ConnectionData> Players
        => _playerIds.Select(cid => _connectionManager.PlayerData(cid));
    public string Location => _gameState.RoomState.Location;

    public override AbstractGameStateDto RoomState => _gameState.RoomState;

    public override async Task HandleOnJoinAsync(CID cid)
    {
        await _gameState.HandleOnJoinAsync(cid);
        await base.HandleOnJoinAsync(cid);
    }

    public override async Task HandleOnLeaveAsync(CID cid)
    {
        await base.HandleOnLeaveAsync(cid);
        await _gameState.HandleOnLeaveAsync(cid);
        if (cid == Owner)
        {
            if (RealPlayerCount > 0)
            {
                Owner = _playerIds.ElementAt(Rand.Next(_playerIds.Count));
                await BroadcastMessageAsync(new OwnerChangeServerMessage() { Data = Owner.Value });
            }
            else
            {
                await _roomManager.CloseLobbyAsync(this);
            }
        }
    }
}
