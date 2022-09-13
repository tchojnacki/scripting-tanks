using MediatR;
using Backend.Services;
using Backend.Contracts.Messages.Server;
using Backend.Mediation.Requests;
using Backend.Utils.Mappings;

namespace Backend.Mediation.Handlers;

public class BroadcastNewPlayerHandler : AsyncRequestHandler<BroadcastNewPlayerRequest>
{
    private readonly IConnectionManager _connectionManager;
    private readonly IRoomManager _roomManager;

    public BroadcastNewPlayerHandler(IConnectionManager connectionManager, IRoomManager roomManager)
    {
        _connectionManager = connectionManager;
        _roomManager = roomManager;
    }

    protected override async Task Handle(BroadcastNewPlayerRequest request, CancellationToken cancellationToken)
    {
        var room = _roomManager.GetRoom(request.LID);
        var dto = _connectionManager.DataFor(request.CID).ToDto();

        await Task.WhenAll(
            room.AllPlayers.Select(player => _connectionManager.SendToSingleAsync(
                player.CID,
                new NewPlayerServerMessage { Data = dto })));
    }
}
