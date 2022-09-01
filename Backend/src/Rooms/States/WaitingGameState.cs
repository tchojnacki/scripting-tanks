using Backend.Contracts.Data;

namespace Backend.Rooms.States;

public class WaitingGameState : GameState
{
    public WaitingGameState(GameRoom gameRoom) : base(gameRoom) { }

    public override GameWaitingStateDto RoomState => new()
    {
        Name = _gameRoom.Name,
        Owner = _gameRoom.Owner.Value,
        Players = _gameRoom.Players,
    };
}
