using MediatR;
using Backend.Services;
using Backend.Contracts.Messages.Server;
using Backend.Mediation.Requests;
using Backend.Utils.Mappings;

namespace Backend.Mediation.Handlers;

internal sealed class BroadcastNewPlayerHandler : AsyncRequestHandler<BroadcastNewPlayerRequest>
{
    private readonly IBroadcastHelper _broadcastHelper;
    private readonly IConnectionManager _connectionManager;

    public BroadcastNewPlayerHandler(IBroadcastHelper broadcastHelper, IConnectionManager connectionManager)
    {
        _broadcastHelper = broadcastHelper;
        _connectionManager = connectionManager;
    }

    protected override Task Handle(BroadcastNewPlayerRequest request, CancellationToken cancellationToken)
        => _broadcastHelper.BroadcastToRoom(
            request.LID,
            new NewPlayerServerMessage { Data = _connectionManager.DataFor(request.CID).ToDto() });
}
