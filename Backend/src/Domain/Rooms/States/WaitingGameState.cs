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

    public override async Task HandleOnMessageAsync(CID cid, IClientMessage message)
    {
        if (cid != _gameRoom.OwnerCID) return;

        await (message switch
        {
            StartGameClientMessage when _gameRoom.AllPlayers.Count() >= 2
                => _gameRoom.StartGameAsync(),
            CloseLobbyClientMessage
                => _gameRoom.CloseLobbyAsync(),
            PromotePlayerClientMessage { Data: var targetString }
                => _gameRoom.PromoteAsync(CID.Deserialize(targetString)),
            KickPlayerClientMessage { Data: var targetString }
                => _gameRoom.KickAsync(CID.Deserialize(targetString)),
            AddBotClientMessage
                => _mediator.Send(new AddBotRequest(_gameRoom.LID)),
            _ => Task.CompletedTask
        });
    }
}
