using MediatR;
using Backend.Domain.Rooms;
using Backend.Domain.Identifiers;
using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Client;
using Backend.Mediation.Requests;

namespace Backend.Services;

public class RoomManager : IRoomManager
{
    private readonly Dictionary<LID, GameRoom> _gameRooms;

    private readonly IMediator _mediator;

    public RoomManager(IMediator mediator)
    {
        _mediator = mediator;

        MenuRoom = new MenuRoom(mediator, this);
        _gameRooms = new();
    }

    public IEnumerable<GameRoom> Lobbies => _gameRooms.Values;

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
        CreateLobbyClientMessage => CreateLobbyAsync(cid),
        EnterLobbyClientMessage { Data: var target } => JoinGameRoomAsync(cid, LID.Deserialize(target)),
        LeaveLobbyClientMessage => KickPlayerAsync(cid),
        _ => RoomContaining(cid).HandleOnMessageAsync(cid, message)
    };

    public Task JoinGameRoomAsync(CID cid, LID lid) => SwitchRoomAsync(cid, _gameRooms[lid]);

    public async Task KickPlayerAsync(CID cid)
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
        _gameRooms[lid] = new(_mediator, cid, lid, name);
        await _mediator.Send(new BroadcastUpsertLobbyRequest(lid));
        await JoinGameRoomAsync(cid, lid);
    }
}
