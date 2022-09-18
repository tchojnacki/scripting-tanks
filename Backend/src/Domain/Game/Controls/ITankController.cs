using Backend.Domain.Rooms.GameStates;

namespace Backend.Domain.Game.Controls;

internal interface ITankController
{
    TankControlsStatus FetchControlsStatus(Tank self, PlayingGameState world);
}
