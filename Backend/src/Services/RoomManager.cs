using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Client;
using Backend.Domain.Identifiers;
using Backend.Domain.Rooms;
using Backend.Domain.Rooms.GameStates;
using Backend.Mediation.Requests;
using MediatR;

namespace Backend.Services;

internal sealed class RoomManager : IRoomManager
{
    private readonly Dictionary<LID, GameRoom> _gameRooms;

    private readonly IMediator _mediator;

    public RoomManager(IMediator mediator)
    {
        _mediator = mediator;

        MenuRoom = new(mediator, this);
        _gameRooms = new();
    }

    public IEnumerable<LobbyInfo> Lobbies => _gameRooms.Values.Select(gr => gr.LobbyInfo);

    public GameRoom GetGameRoom(LID lid) => _gameRooms[lid];

    public bool ContainsGameRoom(LID lid) => _gameRooms.ContainsKey(lid);

    public ConnectionRoom RoomContaining(CID cid)
        => new ConnectionRoom[] { MenuRoom }
            .Concat(_gameRooms.Values.AsEnumerable())
            .Single(r => r.HasPlayer(cid));

    public MenuRoom MenuRoom { get; }

    public async Task CloseLobbyAsync(LID lid)
    {
        var gameRoom = _gameRooms[lid];
        await _mediator.Send(new BroadcastLobbyRemovedRequest(gameRoom.LID));
        _gameRooms.Remove(gameRoom.LID);
        await Task.WhenAll(gameRoom.AllPlayers.Select(p => MenuRoom.HandleOnJoinAsync(p.CID)));
    }

    public Task HandleOnConnectAsync(CID cid) => MenuRoom.HandleOnJoinAsync(cid);

    public Task HandleOnDisconnectAsync(CID cid) => SwitchRoomAsync(cid, null);

    public Task HandleOnMessageAsync(CID cid, IClientMessage message) => message switch
    {
        EnterLobbyClientMessage { Data: var target } => JoinGameRoomAsync(cid, LID.Deserialize(target)),
        CreateLobbyClientMessage => CreateLobbyAsync(cid),
        LeaveLobbyClientMessage => KickPlayerAsync(cid),
        KickPlayerClientMessage { Data: var target } => KickPlayerAsync(CID.Deserialize(target)),
        CloseLobbyClientMessage => CloseLobbyAsync(((GameRoom)RoomContaining(cid)).LID),
        StartGameClientMessage => StartGameAsync(((GameRoom)RoomContaining(cid)).LID),
        _ => RoomContaining(cid).HandleOnMessageAsync(cid, message)
    };

    public Task JoinGameRoomAsync(CID cid, LID lid) => SwitchRoomAsync(cid, _gameRooms[lid]);

    public Task ShowSummaryAsync(LID lid)
        => ChangeGameStateAsync<PlayingGameState, SummaryGameState>(lid, SummaryGameState.AfterPlaying);

    public Task PlayAgainAsync(LID lid)
        => ChangeGameStateAsync<SummaryGameState, WaitingGameState>(lid, WaitingGameState.AfterSummary);

    private async Task KickPlayerAsync(CID cid)
        => await SwitchRoomAsync(cid, (await _mediator.Send(new PlayerDataRequest(cid))).IsBot ? null : MenuRoom);

    private async Task SwitchRoomAsync(CID cid, ConnectionRoom? newRoom)
    {
        await RoomContaining(cid).HandleOnLeaveAsync(cid);

        if (newRoom is not null)
            await newRoom.HandleOnJoinAsync(cid);
    }

    private async Task CreateLobbyAsync(CID cid)
    {
        var data = await _mediator.Send(new PlayerDataRequest(cid));
        var lid = LID.GenerateUnique();
        var name = $"{data.Name}'s Game";
        _gameRooms[lid] = WaitingGameState.CreateNew(_mediator, cid, lid, name);
        await _mediator.Send(new BroadcastUpsertLobbyRequest(lid));
        await JoinGameRoomAsync(cid, lid);
    }

    private async Task ChangeGameStateAsync<TFrom, TTo>(LID lid, Func<TFrom, TTo> converter)
        where TFrom : GameRoom where TTo : GameRoom
    {
        if (_gameRooms.TryGetValue(lid, out var gr) && gr is TFrom previous)
        {
            _gameRooms[lid] = converter(previous);
            await _mediator.Send(new BroadcastRoomStateRequest(lid));
            await _mediator.Send(new BroadcastUpsertLobbyRequest(lid));
        }
    }

    private Task StartGameAsync(LID lid)
        => ChangeGameStateAsync<WaitingGameState, PlayingGameState>(lid, PlayingGameState.AfterWaiting);
}
