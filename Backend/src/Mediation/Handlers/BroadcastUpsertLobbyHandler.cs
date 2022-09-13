using MediatR;
using Backend.Services;
using Backend.Contracts.Messages.Server;
using Backend.Mediation.Requests;
using Backend.Utils.Mappings;

namespace Backend.Mediation.Handlers;

public class BroadcastUpsertLobbyHandler : AsyncRequestHandler<BroadcastUpsertLobbyRequest>
{
    private readonly IConnectionManager _connectionManager;
    private readonly IRoomManager _roomManager;

    public BroadcastUpsertLobbyHandler(IConnectionManager connectionManager, IRoomManager roomManager)
    {
        _connectionManager = connectionManager;
        _roomManager = roomManager;
    }

    protected override async Task Handle(BroadcastUpsertLobbyRequest request, CancellationToken cancellationToken)
    {
        await Task.WhenAll(
            _roomManager.MenuRoom.AllPlayers.Select(player => _connectionManager.SendToSingleAsync(
                player.CID,
                new UpsertLobbyServerMessage { Data = _roomManager.GetRoom(request.LID).ToDto() })));
    }
}
