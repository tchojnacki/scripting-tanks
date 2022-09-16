using MediatR;
using Backend.Services;
using Backend.Domain.Rooms;
using Backend.Contracts.Messages.Server;
using Backend.Mediation.Requests;
using Backend.Utils.Mappings;

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
        ConnectionRoom room = request.LID is not null ? _roomManager.GetGameRoom(request.LID) : _roomManager.MenuRoom;
        await _connectionManager.SendToSingleAsync(
            request.CID,
            new RoomStateServerMessage { Data = room.ToDto() });
    }
}
