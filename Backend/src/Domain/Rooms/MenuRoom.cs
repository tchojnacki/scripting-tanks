using MediatR;
using Backend.Services;

namespace Backend.Domain.Rooms;

internal sealed class MenuRoom : ConnectionRoom
{
    private readonly IRoomManager _roomManager;
    public MenuRoom(IMediator mediator, IRoomManager roomManager) : base(mediator, new())
        => _roomManager = roomManager;

    public IEnumerable<LobbyInfo> Lobbies => _roomManager.Lobbies;
}
