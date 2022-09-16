using MediatR;
using Backend.Services;
using Backend.Mediation.Requests;

namespace Backend.Mediation.Handlers;

internal sealed class CloseLobbyHandler : AsyncRequestHandler<CloseLobbyRequest>
{
    private readonly IRoomManager _roomManager;

    public CloseLobbyHandler(IRoomManager roomManager) => _roomManager = roomManager;

    protected override Task Handle(CloseLobbyRequest request, CancellationToken cancellationToken)
        => _roomManager.CloseLobbyAsync(request.LID);
}
