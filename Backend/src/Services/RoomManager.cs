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

    public GameRoom GetRoom(LID lid) => _gameRooms[lid];

    public MenuRoom MenuRoom { get; }

    public async Task CloseLobbyAsync(LID lid)
    {
        if (_gameRooms.TryGetValue(lid, out var gameRoom))
        {
            await _mediator.Send(new BroadcastLobbyRemovedRequest(gameRoom.LID));
            _gameRooms.Remove(gameRoom.LID);
            await Task.WhenAll(gameRoom.AllPlayers.Select(p => MenuRoom.HandleOnJoinAsync(p.CID)));
        }
    }

    public bool CanPlayerCustomize(CID cid) => RoomContaining(cid) == MenuRoom;

    public Task HandleOnConnectAsync(CID cid) => SwitchRoomAsync(cid, MenuRoom);

    public Task HandleOnDisconnectAsync(CID cid) => SwitchRoomAsync(cid, null);

    public async Task HandleOnMessageAsync(CID cid, IClientMessage message)
    {
        var room = RoomContaining(cid)!;

        await (message switch
        {
            CreateLobbyClientMessage when room == MenuRoom
                => CreateLobbyAsync(cid),
            EnterLobbyClientMessage { Data: var lidString } when room == MenuRoom
                => JoinGameRoomAsync(cid, LID.Deserialize(lidString)),
            LeaveLobbyClientMessage when room != MenuRoom
                => KickPlayerAsync(cid),
            _ => room.HandleOnMessageAsync(cid, message)
        });
    }

    public Task JoinGameRoomAsync(CID cid, LID lid) => SwitchRoomAsync(cid, _gameRooms[lid]);

    public async Task KickPlayerAsync(CID cid)
        => await SwitchRoomAsync(cid, (await _mediator.Send(new PlayerDataRequest(cid))).IsBot ? null : MenuRoom);

    private ConnectionRoom? RoomContaining(CID cid)
        => new ConnectionRoom[] { MenuRoom }
            .Concat(_gameRooms.Values.AsEnumerable())
            .FirstOrDefault(r => r.HasPlayer(cid));

    private async Task SwitchRoomAsync(CID cid, ConnectionRoom? newRoom)
    {
        var previousRoom = RoomContaining(cid);

        if (previousRoom is not null)
            await previousRoom.HandleOnLeaveAsync(cid);

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
