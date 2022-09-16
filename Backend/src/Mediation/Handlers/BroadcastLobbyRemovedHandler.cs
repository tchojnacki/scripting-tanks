using MediatR;
using Backend.Services;
using Backend.Contracts.Messages.Server;
using Backend.Mediation.Requests;

namespace Backend.Mediation.Handlers;

internal sealed class BroadcastLobbyRemovedHandler : AsyncRequestHandler<BroadcastLobbyRemovedRequest>
{
    private readonly IBroadcastHelper _broadcastHelper;

    public BroadcastLobbyRemovedHandler(IBroadcastHelper broadcastHelper) => _broadcastHelper = broadcastHelper;

    protected override Task Handle(BroadcastLobbyRemovedRequest request, CancellationToken cancellationToken)
        => _broadcastHelper.BroadcastToMenu(new LobbyRemovedServerMessage { Data = request.LID.ToString() });
}
