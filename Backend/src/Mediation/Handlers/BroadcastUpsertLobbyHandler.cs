using MediatR;
using Backend.Services;
using Backend.Contracts.Messages.Server;
using Backend.Mediation.Requests;
using Backend.Utils.Mappings;

namespace Backend.Mediation.Handlers;

public class BroadcastUpsertLobbyHandler : AsyncRequestHandler<BroadcastUpsertLobbyRequest>
{
    private readonly IBroadcastHelper _broadcastHelper;
    private readonly IRoomManager _roomManager;

    public BroadcastUpsertLobbyHandler(IBroadcastHelper broadcastHelper, IRoomManager roomManager)
    {
        _broadcastHelper = broadcastHelper;
        _roomManager = roomManager;
    }

    protected override Task Handle(BroadcastUpsertLobbyRequest request, CancellationToken cancellationToken)
        => _broadcastHelper.BroadcastToMenu(
            new UpsertLobbyServerMessage { Data = _roomManager.GetRoom(request.LID).ToDto() });
}
