using MediatR;
using Backend.Services;
using Backend.Contracts.Messages.Server;
using Backend.Mediation.Requests;

namespace Backend.Mediation.Handlers;

internal sealed class BroadcastOwnerChangeHandler : AsyncRequestHandler<BroadcastOwnerChangeRequest>
{
    private readonly IBroadcastHelper _broadcastHelper;

    public BroadcastOwnerChangeHandler(IBroadcastHelper broadcastHelper) => _broadcastHelper = broadcastHelper;

    protected override Task Handle(BroadcastOwnerChangeRequest request, CancellationToken cancellationToken)
        => _broadcastHelper.BroadcastToRoom(
            request.LID,
            new OwnerChangeServerMessage { Data = request.NewOwnerCID.ToString() });
}
