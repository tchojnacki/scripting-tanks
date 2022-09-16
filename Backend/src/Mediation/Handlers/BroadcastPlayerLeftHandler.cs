using MediatR;
using Backend.Services;
using Backend.Contracts.Messages.Server;
using Backend.Mediation.Requests;

namespace Backend.Mediation.Handlers;

internal sealed class BroadcastPlayerLeftHandler : AsyncRequestHandler<BroadcastPlayerLeftRequest>
{
    private readonly IBroadcastHelper _broadcastHelper;

    public BroadcastPlayerLeftHandler(IBroadcastHelper broadcastHelper) => _broadcastHelper = broadcastHelper;

    protected override Task Handle(BroadcastPlayerLeftRequest request, CancellationToken cancellationToken)
        => _broadcastHelper.BroadcastToRoom(
            request.LID,
            new PlayerLeftServerMessage { Data = request.CID.ToString() });
}
