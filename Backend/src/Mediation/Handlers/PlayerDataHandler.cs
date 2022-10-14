using Backend.Domain;
using Backend.Mediation.Requests;
using Backend.Services;
using MediatR;

namespace Backend.Mediation.Handlers;

internal sealed class PlayerDataHandler : RequestHandler<PlayerDataRequest, PlayerData>
{
    private readonly IConnectionManager _connectionManager;

    public PlayerDataHandler(IConnectionManager connectionManager) => _connectionManager = connectionManager;

    protected override PlayerData Handle(PlayerDataRequest request) => _connectionManager.DataFor(request.Cid);
}