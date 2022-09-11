using Backend.Utils.Mappings;
using Backend.Identifiers;
using Backend.Contracts.Data;
using Backend.Contracts.Messages;
using Backend.Contracts.Messages.Client;
using Backend.Contracts.Messages.Server;

namespace Backend.Rooms.States;

public class WaitingGameState : GameState
{
    public WaitingGameState(GameRoom gameRoom) : base(gameRoom) { }

    public override GameWaitingStateDto RoomState => new()
    {
        Name = _gameRoom.Name,
        Owner = _gameRoom.Owner.Value,
        Players = _gameRoom.Players.Select(p => p.ToDto()).ToList(),
    };

    public override Task HandleOnJoinAsync(CID cid)
        => _gameRoom.BroadcastMessageAsync(new NewPlayerServerMessage { Data = _gameRoom.DataFor(cid).ToDto() });

    public override Task HandleOnLeaveAsync(CID cid)
        => _gameRoom.BroadcastMessageAsync(new PlayerLeftServerMessage { Data = cid.Value });

    public override async Task HandleOnMessageAsync(CID cid, IClientMessage message)
    {
        if (cid != _gameRoom.Owner) return;

        await (message switch
        {
            StartGameClientMessage when _gameRoom.Players.Count() >= 2
                => _gameRoom.StartGameAsync(),
            CloseLobbyClientMessage
                => _gameRoom.CloseLobbyAsync(),
            PromotePlayerClientMessage { Data: var targetString }
                => _gameRoom.PromoteAsync(CID.From(targetString)),
            KickPlayerClientMessage { Data: var targetString }
                => _gameRoom.KickAsync(CID.From(targetString)),
            AddBotClientMessage
                => _gameRoom.AddBotAsync(),
            _ => Task.CompletedTask
        });
    }
}
