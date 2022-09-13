using MediatR;
using Backend.Services;
using Backend.Mediation.Requests;

namespace Backend.Mediation.Handlers;

public class KickHandler : AsyncRequestHandler<KickRequest>
{
    private readonly IRoomManager _roomManager;

    public KickHandler(IRoomManager roomManager) => _roomManager = roomManager;

    protected override Task Handle(KickRequest request, CancellationToken cancellationToken)
        => _roomManager.KickPlayerAsync(request.CID);
}
