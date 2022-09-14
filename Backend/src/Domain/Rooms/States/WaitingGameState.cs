using MediatR;
using Backend.Utils.Mappings;
using Backend.Domain.Identifiers;
using Backend.Contracts.Data;
using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Client;
using Backend.Mediation.Requests;

namespace Backend.Domain.Rooms.States;

public class WaitingGameState : GameState
{
    public WaitingGameState(IMediator mediator, GameRoom gameRoom) : base(mediator, gameRoom) { }

    public override GameWaitingStateDto RoomState => new()
    {
        Name = _gameRoom.Name,
        Owner = _gameRoom.OwnerCID.ToString(),
        Players = _gameRoom.AllPlayers.Select(p => p.ToDto()).ToList(),
    };

    public override async Task HandleOnJoinAsync(CID cid)
        => await _mediator.Send(new BroadcastNewPlayerRequest(_gameRoom.LID, cid));

    public override Task HandleOnLeaveAsync(CID cid)
        => _mediator.Send(new BroadcastPlayerLeftRequest(_gameRoom.LID, cid));

    public override Task HandleOnMessageAsync(CID cid, IClientMessage message) => message switch
    {
        StartGameClientMessage => _gameRoom.StartGameAsync(),
        CloseLobbyClientMessage => _mediator.Send(new CloseLobbyRequest(_gameRoom.LID)),
        PromotePlayerClientMessage { Data: var target } => _gameRoom.PromoteAsync(CID.Deserialize(target)),
        KickPlayerClientMessage { Data: var target } => _mediator.Send(new KickRequest(CID.Deserialize(target))),
        AddBotClientMessage => _mediator.Send(new AddBotRequest(_gameRoom.LID)),
        _ => Task.CompletedTask
    };
}
