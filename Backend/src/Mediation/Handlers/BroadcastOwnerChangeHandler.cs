using MediatR;
using Backend.Services;
using Backend.Contracts.Messages.Server;
using Backend.Mediation.Requests;
using Backend.Utils.Mappings;

namespace Backend.Mediation.Handlers;

public class BroadcastOwnerChangeHandler : AsyncRequestHandler<BroadcastOwnerChangeRequest>
{
    private readonly IConnectionManager _connectionManager;
    private readonly IRoomManager _roomManager;

    public BroadcastOwnerChangeHandler(IConnectionManager connectionManager, IRoomManager roomManager)
    {
        _connectionManager = connectionManager;
        _roomManager = roomManager;
    }

    protected override async Task Handle(BroadcastOwnerChangeRequest request, CancellationToken cancellationToken)
    {
        var room = _roomManager.GetRoom(request.LID);

        await Task.WhenAll(
            room.AllPlayers.Select(player => _connectionManager.SendToSingleAsync(
                player.CID,
                new OwnerChangeServerMessage { Data = request.NewOwnerCID.ToString() })));
    }
}
