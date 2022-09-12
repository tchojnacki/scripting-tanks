using Backend.Services;
using Backend.Domain;
using Backend.Domain.Identifiers;
using Backend.Contracts.Data;
using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Server;
using Backend.Rooms.States;

namespace Backend.Rooms;

public class GameRoom : ConnectionRoom
{
    private static readonly Random Rand = new();

    private GameState _gameState;

    public GameRoom(
        IConnectionManager connectionManager,
        RoomManager roomManager,
        CID owner,
        LID lid,
        string name)
        : base(connectionManager, roomManager)
    {
        LID = lid;
        OwnerCID = owner;
        Name = name;
        _gameState = new WaitingGameState(this);
    }

    public LID LID { get; }
    public string Name { get; }
    public CID OwnerCID { get; private set; }

    public IEnumerable<PlayerData> Players => _playerIds.Select(cid => _connectionManager.DataFor(cid));
    public IEnumerable<PlayerData> RealPlayers => Players.Where(p => !p.IsBot);
    public string Location => _gameState.RoomState.Location;

    public override AbstractRoomStateDto RoomState => _gameState.RoomState;

    public PlayerData DataFor(CID cid) => _connectionManager.DataFor(cid);

    private async Task SwitchStateAsync<TFrom>(GameState newState)
        where TFrom : GameState
    {
        if (_gameState is TFrom)
        {
            _gameState = newState;
            await BroadcastMessageAsync(new RoomStateServerMessage { Data = RoomState });
            await _roomManager.UpsertLobbyAsync(this);
        }
    }

    public Task StartGameAsync() => SwitchStateAsync<WaitingGameState>(new PlayingGameState(this));

    public Task ShowSummary(IReadOnlyScoreboard scoreboard)
        => SwitchStateAsync<PlayingGameState>(new SummaryGameState(this, scoreboard));

    public Task PlayAgain() => SwitchStateAsync<SummaryGameState>(new WaitingGameState(this));

    public Task CloseLobbyAsync() => _roomManager.CloseLobbyAsync(this);

    public async Task PromoteAsync(CID cid)
    {
        if (HasPlayer(cid) && !_connectionManager.DataFor(cid).IsBot)
        {
            OwnerCID = cid;
            await BroadcastMessageAsync(new OwnerChangeServerMessage() { Data = OwnerCID.ToString() });
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
        await _roomManager.JoinGameRoomAsync(cid, LID);
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
        if (cid == OwnerCID)
        {
            var ownerCandidates = RealPlayers.Select(p => p.CID).ToList();
            if (ownerCandidates.Any())
            {
                OwnerCID = ownerCandidates.ElementAt(Rand.Next(ownerCandidates.Count));
                await BroadcastMessageAsync(new OwnerChangeServerMessage() { Data = OwnerCID.ToString() });
            }
            else
            {
                await _roomManager.CloseLobbyAsync(this);
            }
        }
    }

    public override Task HandleOnMessageAsync(CID cid, IClientMessage message)
        => _gameState.HandleOnMessageAsync(cid, message);
}
