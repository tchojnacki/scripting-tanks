from __future__ import annotations
from math import pi, sin, cos, sqrt
from time import monotonic
import asyncio
from typing import TYPE_CHECKING
from dto import FullGamePlayingStateDto
from messages.client import ClientMsg, CSetInputAxesMsg
from messages.server.full_room_state import SFullRoomStateMsg
from models import Vector, Tank
from utils.assign_color import assign_color
from utils.uid import CID, get_eid
from .game_state import GameState

if TYPE_CHECKING:
    from rooms.game_room import GameRoom

TICK_RATE = 24
PLAYER_DISTANCE = 2048
ISLAND_MARGIN = 128


class PlayingGameState(GameState):
    def __init__(self, room: GameRoom):
        super().__init__(room)

        self._last_update = monotonic()

        player_count = len(self._room.players)
        step = (2 * pi) / player_count
        self.radius = int(
            PLAYER_DISTANCE / 2 if player_count == 1 else
            PLAYER_DISTANCE / sqrt(2 - 2 * cos(2 * pi / player_count))
        ) + ISLAND_MARGIN

        self.entities = {
            (tank := Tank(
                world=self,
                cid=player.cid,
                color=assign_color(player.cid),
                pos=Vector(
                    sin(step * i) * (self.radius - ISLAND_MARGIN),
                    0,
                    cos(step * i) * (self.radius - ISLAND_MARGIN),
                ),
                pitch=step * i + pi
            )).eid: tank
            for i, player in enumerate(self._room.players)
        }

        asyncio.ensure_future(self._loop())

    async def _loop(self):
        now = monotonic()
        dtime = now - self._last_update
        self._last_update = now

        delete_queue = set()
        for entity in self.entities.values():
            entity.update(dtime)
            if entity.highest_point < -100:
                delete_queue.add(entity.eid)

        for eid in delete_queue:
            del self.entities[eid]

        await self._room.broadcast_message(SFullRoomStateMsg(self.get_full_room_state()))

        await asyncio.sleep(1 / TICK_RATE)
        if len(self._room.players) > 0:
            asyncio.ensure_future(self._loop())

    async def on_leave(self, leaver_cid: CID):
        if (eid := get_eid(leaver_cid)) in self.entities:
            del self.entities[eid]

    def get_full_room_state(self) -> FullGamePlayingStateDto:
        return FullGamePlayingStateDto(self.radius, [e.to_dto() for e in self.entities.values()])

    def handle_message(self, sender_cid: CID, cmsg: ClientMsg):
        if (eid := get_eid(sender_cid)) in self.entities:
            match cmsg:
                case CSetInputAxesMsg(new_axes):
                    self.entities[eid].input_axes = new_axes
