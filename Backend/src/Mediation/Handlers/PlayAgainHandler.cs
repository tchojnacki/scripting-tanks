using MediatR;
using Backend.Services;
using Backend.Mediation.Requests;

namespace Backend.Mediation.Handlers;

internal sealed class PlayAgainHandler : AsyncRequestHandler<PlayAgainRequest>
{
    private readonly IRoomManager _roomManager;

    public PlayAgainHandler(IRoomManager roomManager) => _roomManager = roomManager;

    protected override Task Handle(PlayAgainRequest request, CancellationToken cancellationToken)
        => _roomManager.PlayAgainAsync(request.LID);
}
