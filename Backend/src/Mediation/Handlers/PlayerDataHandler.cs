using MediatR;
using Backend.Services;
using Backend.Domain;
using Backend.Mediation.Requests;

namespace Backend.Mediation.Handlers;

internal sealed class PlayerDataHandler : RequestHandler<PlayerDataRequest, PlayerData>
{
    private readonly IConnectionManager _connectionManager;

    public PlayerDataHandler(IConnectionManager connectionManager) => _connectionManager = connectionManager;

    protected override PlayerData Handle(PlayerDataRequest request) => _connectionManager.DataFor(request.CID);
}
