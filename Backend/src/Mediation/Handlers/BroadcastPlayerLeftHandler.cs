using MediatR;
using Backend.Services;
using Backend.Contracts.Messages.Server;
using Backend.Mediation.Requests;

namespace Backend.Mediation.Handlers;

public class BroadcastPlayerLeftHandler : AsyncRequestHandler<BroadcastPlayerLeftRequest>
{
    private readonly IConnectionManager _connectionManager;
    private readonly IRoomManager _roomManager;

    public BroadcastPlayerLeftHandler(IConnectionManager connectionManager, IRoomManager roomManager)
    {
        _connectionManager = connectionManager;
        _roomManager = roomManager;
    }

    protected override async Task Handle(BroadcastPlayerLeftRequest request, CancellationToken cancellationToken)
    {
        var room = _roomManager.GetRoom(request.LID);

        await Task.WhenAll(
            room.AllPlayers.Select(player => _connectionManager.SendToSingleAsync(
                player.CID,
                new PlayerLeftServerMessage { Data = request.CID.ToString() })));
    }
}
