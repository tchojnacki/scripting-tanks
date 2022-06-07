from __future__ import annotations
import asyncio
from math import cos, pi, sin
from typing import TYPE_CHECKING
from dto import FullGameSummaryStateDto,  ScoreboardEntryDto, TankDataDto
from messages.server import SFullRoomStateMsg
from models import Vector, Tank
from .game_state import GameState

if TYPE_CHECKING:
    from rooms.game_room import GameRoom

PODIUM_RADIUS = 512
PODIUM_HEIGHT = 32
SUMMARY_DURATION = 10


class SummaryGameState(GameState):
    def __init__(self, room: GameRoom, *, scoreboard: list[ScoreboardEntryDto]):
        super().__init__(room)
        self._scoreboard = scoreboard
        self._remaining = SUMMARY_DURATION
        asyncio.ensure_future(self._wait_to_play_again())

    @property
    def _tanks(self) -> list[TankDataDto]:
        return [
            Tank(
                world=self,
                cid=entry.cid,
                name=(data := self._room.cid_to_player_data(entry.cid)).name,
                colors=data.colors,
                pos=Vector(
                    PODIUM_RADIUS * sin(angle := i * pi/6 + 5*pi/6),
                    PODIUM_HEIGHT * (3 - i),
                    PODIUM_RADIUS * cos(angle)
                ),
                pitch=pi + angle
            ).to_dto() for (i, entry) in enumerate(self._scoreboard[:3])
        ]

    async def _wait_to_play_again(self):
        while self._remaining > 0:
            await asyncio.sleep(1)
            self._remaining -= 1
            await self._room.broadcast_message(SFullRoomStateMsg(self.get_full_room_state()))
        await self._room.play_again()

    def get_full_room_state(self) -> FullGameSummaryStateDto:
        return FullGameSummaryStateDto(
            self._remaining,
            self._tanks,
            self._scoreboard
        )
