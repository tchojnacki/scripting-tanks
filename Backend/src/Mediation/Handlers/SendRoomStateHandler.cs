using Backend.Contracts.Messages.Server;
using Backend.Domain.Rooms;
using Backend.Mediation.Requests;
using Backend.Services;
using Backend.Utils.Mappings;
using MediatR;

namespace Backend.Mediation.Handlers;

internal sealed class SendRoomStateHandler : AsyncRequestHandler<SendRoomStateRequest>
{
    private readonly IConnectionManager _connectionManager;
    private readonly IRoomManager _roomManager;

    public SendRoomStateHandler(IConnectionManager connectionManager, IRoomManager roomManager)
    {
        _connectionManager = connectionManager;
        _roomManager = roomManager;
    }

    protected override async Task Handle(SendRoomStateRequest request, CancellationToken cancellationToken)
    {
        ConnectionRoom room = request.Lid is not null ? _roomManager.GetGameRoom(request.Lid) : _roomManager.MenuRoom;
        await _connectionManager.SendToSingleAsync(
            request.Cid,
            new RoomStateServerMessage { Data = room.ToDto() });
    }
}