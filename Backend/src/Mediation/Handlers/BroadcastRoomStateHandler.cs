using MediatR;
using Backend.Services;
using Backend.Contracts.Messages.Server;
using Backend.Mediation.Requests;
using Backend.Utils.Mappings;

namespace Backend.Mediation.Handlers;

internal sealed class BroadcastRoomStateHandler : AsyncRequestHandler<BroadcastRoomStateRequest>
{
    private readonly IBroadcastHelper _broadcastHelper;
    private readonly IRoomManager _roomManager;

    public BroadcastRoomStateHandler(IBroadcastHelper broadcastHelper, IRoomManager roomManager)
    {
        _broadcastHelper = broadcastHelper;
        _roomManager = roomManager;
    }

    protected override Task Handle(BroadcastRoomStateRequest request, CancellationToken cancellationToken)
        => _broadcastHelper.BroadcastToRoom(
            request.LID,
            new RoomStateServerMessage { Data = _roomManager.GetGameRoom(request.LID).ToDto() });
}
