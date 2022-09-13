using MediatR;
using Backend.Services;
using Backend.Contracts.Data;
using Backend.Utils.Mappings;

namespace Backend.Domain.Rooms;

public class MenuRoom : ConnectionRoom
{
    public MenuRoom(IMediator mediator, IRoomManager roomManager)
        : base(mediator, roomManager) { }

    public override MenuStateDto RoomState => new()
    {
        Lobbies = _roomManager.Lobbies.Select(l => l.ToDto()).ToList()
    };
}
