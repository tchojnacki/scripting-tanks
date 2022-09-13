using MediatR;
using Backend.Services;
using Backend.Contracts.Messages.Server;
using Backend.Mediation.Requests;

namespace Backend.Mediation.Handlers;

public class BroadcastLobbyRemovedHandler : AsyncRequestHandler<BroadcastLobbyRemovedRequest>
{
    private readonly IConnectionManager _connectionManager;
    private readonly IRoomManager _roomManager;

    public BroadcastLobbyRemovedHandler(IConnectionManager connectionManager, IRoomManager roomManager)
    {
        _connectionManager = connectionManager;
        _roomManager = roomManager;
    }

    protected override async Task Handle(BroadcastLobbyRemovedRequest request, CancellationToken cancellationToken)
    {
        var room = _roomManager.GetRoom(request.LID);

        await Task.WhenAll(
            room.AllPlayers.Select(player => _connectionManager.SendToSingleAsync(
                player.CID,
                new LobbyRemovedServerMessage { Data = request.LID.ToString() })));
    }
}
