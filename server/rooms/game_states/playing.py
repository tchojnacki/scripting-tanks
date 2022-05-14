from math import pi, sin, cos
from typing import TYPE_CHECKING
from dto import FullGamePlayingStateDto, EntityDataDto
from utils.assign_color import assign_color
from utils.uid import get_eid
from .game_state import GameState

if TYPE_CHECKING:
    from rooms.game_room import GameRoom


class PlayingGameState(GameState):
    def __init__(self, room: "GameRoom"):
        super().__init__(room)

        step = (2 * pi) / len(self._room.players)
        radius = 10

        self.entities = [
            EntityDataDto(
                eid := get_eid(player.cid),
                assign_color(eid),
                sin(step * i) * radius,
                cos(step * i) * radius
            )
            for i, player in enumerate(self._room.players)
        ]

    def get_full_room_state(self) -> FullGamePlayingStateDto:
        return FullGamePlayingStateDto(self.entities)
