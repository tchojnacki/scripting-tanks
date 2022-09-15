using MediatR;
using Backend.Utils.Mappings;
using Backend.Domain.Identifiers;
using Backend.Contracts.Data;
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
}
