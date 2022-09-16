using MediatR;
using Backend.Services;
using Backend.Mediation.Requests;

namespace Backend.Mediation.Handlers;

public class ShowSummaryHandler : AsyncRequestHandler<ShowSummaryRequest>
{
    private readonly IRoomManager _roomManager;

    public ShowSummaryHandler(IRoomManager roomManager) => _roomManager = roomManager;

    protected override Task Handle(ShowSummaryRequest request, CancellationToken cancellationToken)
        => _roomManager.ShowSummaryAsync(request.LID);
}
