using MediatR;
using Backend.Services;
using Backend.Domain.Identifiers;
using Backend.Contracts.Data;
using Backend.Contracts.Messages;
using Backend.Utils.Mappings;

namespace Backend.Domain.Rooms;

public class MenuRoom : ConnectionRoom
{
    private readonly IRoomManager _roomManager;
    public MenuRoom(IMediator mediator, IRoomManager roomManager) : base(mediator, new())
        => _roomManager = roomManager;

    public override MenuStateDto RoomState => new()
    {
        Lobbies = _roomManager.Lobbies.Select(l => l.ToDto()).ToList()
    };

    public override Task HandleOnMessageAsync(CID cid, IClientMessage message) => Task.CompletedTask;
}
