from __future__ import annotations
from math import pi, sin, cos, sqrt
import asyncio
from typing import TYPE_CHECKING
from dto import FullGamePlayingStateDto, InputAxesDto
from messages.client import ClientMsg, CSetInputAxesMsg
from messages.server import SEntityUpdateMsg
from models import Vector, Tank
from utils.assign_color import assign_color
from utils.uid import CID, get_eid
from .game_state import GameState

if TYPE_CHECKING:
    from rooms.game_room import GameRoom

TICK_RATE = 24
PLAYER_DISTANCE = 1024
ISLAND_MARGIN = 128
MOVE_SPEED = 10
TURN_SPEED = 0.025


class PlayingGameState(GameState):
    def __init__(self, room: GameRoom):
        super().__init__(room)

        player_count = len(self._room.players)
        step = (2 * pi) / player_count
        self.radius = int(
            PLAYER_DISTANCE / 2 if player_count == 1 else
            PLAYER_DISTANCE / sqrt(2 - 2 * cos(2 * pi / player_count))
        ) + ISLAND_MARGIN

        self.entities = [
            Tank(
                cid=player.cid,
                color=assign_color(player.cid),
                pos=Vector(
                    sin(step * i) * (self.radius - ISLAND_MARGIN),
                    0,
                    cos(step * i) * (self.radius - ISLAND_MARGIN),
                ),
                pitch=step * i + pi
            )
            for i, player in enumerate(self._room.players)
        ]

        self._input_axes: dict[CID, InputAxesDto] = {
            (player.cid): InputAxesDto(0, 0) for player in self._room.players
        }

        asyncio.ensure_future(self._loop())

    async def _loop(self):
        for (cid, axes) in self._input_axes.items():
            eid = get_eid(cid)
            entity = next(e for e in self.entities if e.eid == eid)

            entity.pitch -= axes.horizontal * axes.vertical * TURN_SPEED
            entity.pos.x += sin(entity.pitch) * axes.vertical * MOVE_SPEED
            entity.pos.z += cos(entity.pitch) * axes.vertical * MOVE_SPEED

            await self._room.broadcast_message(SEntityUpdateMsg(entity.to_dto()))

        await asyncio.sleep(1 / TICK_RATE)
        if len(self._room.players) > 0:
            asyncio.ensure_future(self._loop())

    def get_full_room_state(self) -> FullGamePlayingStateDto:
        return FullGamePlayingStateDto(self.radius, [e.to_dto() for e in self.entities])

    def handle_message(self, sender_cid: CID, cmsg: ClientMsg):
        match cmsg:
            case CSetInputAxesMsg(new_axes):
                self._input_axes[sender_cid] = new_axes
