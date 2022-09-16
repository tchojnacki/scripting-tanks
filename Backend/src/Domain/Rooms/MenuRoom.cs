using MediatR;
using Backend.Services;
using Backend.Contracts.Data;
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
}
