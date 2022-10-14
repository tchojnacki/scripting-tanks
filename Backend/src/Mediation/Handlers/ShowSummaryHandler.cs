using Backend.Mediation.Requests;
using Backend.Services;
using MediatR;

namespace Backend.Mediation.Handlers;

internal sealed class ShowSummaryHandler : AsyncRequestHandler<ShowSummaryRequest>
{
    private readonly IRoomManager _roomManager;

    public ShowSummaryHandler(IRoomManager roomManager) => _roomManager = roomManager;

    protected override Task Handle(ShowSummaryRequest request, CancellationToken cancellationToken)
        => _roomManager.ShowSummaryAsync(request.Lid);
}