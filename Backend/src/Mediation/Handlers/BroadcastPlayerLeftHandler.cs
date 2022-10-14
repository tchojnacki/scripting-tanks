using Backend.Contracts.Messages.Server;
using Backend.Mediation.Requests;
using Backend.Services;
using MediatR;

namespace Backend.Mediation.Handlers;

internal sealed class BroadcastPlayerLeftHandler : AsyncRequestHandler<BroadcastPlayerLeftRequest>
{
    private readonly IBroadcastHelper _broadcastHelper;

    public BroadcastPlayerLeftHandler(IBroadcastHelper broadcastHelper) => _broadcastHelper = broadcastHelper;

    protected override Task Handle(BroadcastPlayerLeftRequest request, CancellationToken cancellationToken)
        => _broadcastHelper.BroadcastToRoom(
            request.Lid,
            new PlayerLeftServerMessage { Data = request.Cid.ToString() });
}