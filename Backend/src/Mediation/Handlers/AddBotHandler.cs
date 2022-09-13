using MediatR;
using Backend.Services;
using Backend.Mediation.Requests;

namespace Backend.Mediation.Handlers;

public class AddBotHandler : AsyncRequestHandler<AddBotRequest>
{
    private readonly IConnectionManager _connectionManager;

    public AddBotHandler(IConnectionManager connectionManager) => _connectionManager = connectionManager;

    protected override Task Handle(AddBotRequest request, CancellationToken cancellationToken)
        => _connectionManager.AddBotAsync(request.LID);
}
