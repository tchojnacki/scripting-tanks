using Backend.Contracts.Messages.Server;
using Backend.Mediation.Requests;
using Backend.Services;
using MediatR;

namespace Backend.Mediation.Handlers;

internal sealed class BroadcastOwnerChangeHandler : AsyncRequestHandler<BroadcastOwnerChangeRequest>
{
    private readonly IBroadcastHelper _broadcastHelper;

    public BroadcastOwnerChangeHandler(IBroadcastHelper broadcastHelper) => _broadcastHelper = broadcastHelper;

    protected override Task Handle(BroadcastOwnerChangeRequest request, CancellationToken cancellationToken)
        => _broadcastHelper.BroadcastToRoom(
            request.Lid,
            new OwnerChangeServerMessage { Data = request.NewOwnerCid.ToString() });
}