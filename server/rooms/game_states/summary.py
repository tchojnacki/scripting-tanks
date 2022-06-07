from __future__ import annotations
import asyncio
from math import cos, pi, sin
from typing import TYPE_CHECKING
from dto import FullGameSummaryStateDto,  ScoreboardEntryDto
from messages.server import SFullRoomStateMsg
from models import Vector, Tank, Entity
from utils.color import assign_color
from utils.uid import EID, get_cid
from .game_state import GameState

if TYPE_CHECKING:
    from rooms.game_room import GameRoom

PODIUM_RADIUS = 512
PODIUM_HEIGHT = 32
SUMMARY_DURATION = 10


class SummaryGameState(GameState):
    def __init__(self, room: GameRoom, *, scoreboard: list[ScoreboardEntryDto]):
        super().__init__(room)

        self._tanks: dict[EID, Entity] = {
            (tank := Tank(
                world=self,
                cid=(cid := get_cid(i)),
                colors=[assign_color(cid), assign_color(cid)],
                pos=Vector(
                    PODIUM_RADIUS * sin(angle := i * pi/6 + 5*pi/6),
                    PODIUM_HEIGHT * (i + 1),
                    PODIUM_RADIUS * cos(angle)
                ),
                pitch=pi + angle
            )).eid: tank
            for i in range(3)
        }

        self._scoreboard = scoreboard

        self._remaining = SUMMARY_DURATION
        asyncio.ensure_future(self._wait_to_play_again())

    async def _wait_to_play_again(self):
        while self._remaining > 0:
            await asyncio.sleep(1)
            self._remaining -= 1
            await self._room.broadcast_message(SFullRoomStateMsg(self.get_full_room_state()))
        await self._room.play_again()

    def get_full_room_state(self) -> FullGameSummaryStateDto:
        return FullGameSummaryStateDto(
            self._remaining,
            [t.to_dto() for t in self._tanks.values()],
            self._scoreboard
        )
