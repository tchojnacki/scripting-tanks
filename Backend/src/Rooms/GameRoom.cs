using Backend.Services;
using Backend.Identifiers;
using Backend.Domain;
using Backend.Contracts.Data;
using Backend.Contracts.Messages;
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

    public IEnumerable<ConnectionData> Players
        => _playerIds.Select(cid => _connectionManager.PlayerData(cid));
    public IEnumerable<ConnectionData> RealPlayers
        => Players.Where(p => !p.IsBot);
    public string Location => _gameState.RoomState.Location;

    public override AbstractGameStateDto RoomState => _gameState.RoomState;

    public ConnectionData PlayerData(CID cid) => _connectionManager.PlayerData(cid);

    public Task StartGameAsync() => Task.CompletedTask;

    public Task CloseLobbyAsync() => _roomManager.CloseLobbyAsync(this);

    public async Task PromoteAsync(CID cid)
    {
        if (HasPlayer(cid) && !_connectionManager.PlayerData(cid).IsBot)
        {
            Owner = cid;
            await BroadcastMessageAsync(new OwnerChangeServerMessage() { Data = Owner.Value });
        }
    }

    public async Task KickAsync(CID cid)
    {
        if (HasPlayer(cid))
            await _roomManager.KickPlayerAsync(cid);
    }

    public async Task AddBotAsync()
    {
        var cid = await _connectionManager.AddBotAsync();
        await _roomManager.JoinGameRoomAsync(cid, Lid);
    }

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
            var ownerCandidates = RealPlayers.Select(p => p.Cid).ToList();
            if (ownerCandidates.Any())
            {
                Owner = ownerCandidates.ElementAt(Rand.Next(ownerCandidates.Count));
                await BroadcastMessageAsync(new OwnerChangeServerMessage() { Data = Owner.Value });
            }
            else
            {
                await _roomManager.CloseLobbyAsync(this);
            }
        }
    }

    public override Task HandleOnMessageAsync(CID cid, IClientMessage<object?> message)
        => _gameState.HandleOnMessageAsync(cid, message);
}
