from math import pi, sin, cos, sqrt
import asyncio
from typing import TYPE_CHECKING
from attr import evolve
from dto import FullGamePlayingStateDto, InputAxesDto, EntityDataDto
from messages.client import ClientMsg, CSetInputAxesMsg
from messages.server import SEntityUpdateMsg
from utils.assign_color import assign_color
from utils.uid import CID, get_eid
from .game_state import GameState

if TYPE_CHECKING:
    from rooms.game_room import GameRoom

FPS = 30
PLAYER_DISTANCE = 1024
ISLAND_MARGIN = 64
MOVE_SPEED = 10
TURN_SPEED = 0.05


class PlayingGameState(GameState):
    def __init__(self, room: "GameRoom"):
        super().__init__(room)

        player_count = len(self._room.players)
        step = (2 * pi) / player_count
        self.radius = int(
            PLAYER_DISTANCE / 2 if player_count == 1 else
            PLAYER_DISTANCE / sqrt(2 - 2 * cos(2 * pi / player_count))
        ) + ISLAND_MARGIN

        self.entities = [
            EntityDataDto(
                eid := get_eid(player.cid),
                player.cid,
                "tank",
                assign_color(eid),
                sin(step * i) * (self.radius - ISLAND_MARGIN),
                cos(step * i) * (self.radius - ISLAND_MARGIN),
                step * i + pi
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
            idx, old_entity = next(
                entry for entry
                in enumerate(self.entities)
                if entry[1].eid == eid
            )

            new_entity = evolve(
                old_entity,
                pitch=old_entity.pitch - axes.horizontal * TURN_SPEED,
                x=old_entity.x + sin(old_entity.pitch) * axes.vertical * MOVE_SPEED,
                z=old_entity.z + cos(old_entity.pitch) * axes.vertical * MOVE_SPEED
            )

            if old_entity != new_entity:
                self.entities[idx] = new_entity
                await self._room.broadcast_message(SEntityUpdateMsg(new_entity))

        await asyncio.sleep(1 / FPS)
        if len(self._room.players) > 0:
            asyncio.ensure_future(self._loop())

    def get_full_room_state(self) -> FullGamePlayingStateDto:
        return FullGamePlayingStateDto(self.radius, self.entities)

    def handle_message(self, sender_cid: CID, cmsg: ClientMsg):
        match cmsg:
            case CSetInputAxesMsg(new_axes):
                self._input_axes[sender_cid] = new_axes
