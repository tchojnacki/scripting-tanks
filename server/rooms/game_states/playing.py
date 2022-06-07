from __future__ import annotations
from math import pi, sin, cos, sqrt
from time import monotonic
import asyncio
from typing import TYPE_CHECKING
from dto import FullGamePlayingStateDto, ScoreboardEntryDto
from messages.client import ClientMsg, CSetInputAxesMsg, CSetBarrelTargetMsg, CShootMsg
from messages.server import SFullRoomStateMsg
from models import Vector, Tank, Entity
from utils.uid import CID, EID, get_eid
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

        self._entities: dict[EID, Entity] = {
            (tank := Tank(
                world=self,
                cid=player.cid,
                name=player.name,
                colors=self._room.cid_to_player_data(player.cid).colors,
                pos=Vector(
                    sin(step * i) * (self.radius - ISLAND_MARGIN),
                    0,
                    cos(step * i) * (self.radius - ISLAND_MARGIN),
                ),
                pitch=step * i + pi
            )).eid: tank
            for i, player in enumerate(self._room.players)
        }

        self._scoreboard: dict[CID, Entity] = {player.cid: 0 for player in self._room.players}

        self._destroy_queue: set[EID] = set()

        asyncio.ensure_future(self._loop())

    async def _loop(self):
        now = monotonic()
        dtime = now - self._last_update
        self._last_update = now

        for entity in self._entities.values():
            entity.update(dtime)

        for (eid1, ent1) in self._entities.items():
            for (eid2, ent2) in self._entities.items():
                if eid2 > eid1 and (ent2.pos - ent1.pos).length < ent1.radius + ent2.radius:
                    if isinstance(ent1, Tank):
                        ent1.collide_with(ent2)
                    elif isinstance(ent2, Tank):
                        ent2.collide_with(ent1)

        for eid in self._destroy_queue:
            del self._entities[eid]
        self._destroy_queue.clear()

        await self._room.broadcast_message(SFullRoomStateMsg(self.get_full_room_state()))

        await asyncio.sleep(1 / TICK_RATE)
        if len([p for p in self._entities.values() if isinstance(p, Tank)]) >= 2:
            asyncio.ensure_future(self._loop())
        else:
            await self._room.show_summary(self._scoreboard_entries)

    def spawn(self, entity: Entity):
        self._entities[entity.eid] = entity

    def destroy(self, entity: Entity):
        self._destroy_queue.add(entity.eid)

    def grant_point(self, cid: CID):
        self._scoreboard[cid] += 1

    async def on_leave(self, leaver_cid: CID):
        if (eid := get_eid(leaver_cid)) in self._entities:
            del self._entities[eid]
        if leaver_cid in self._scoreboard:
            del self._scoreboard[leaver_cid]

    @property
    def _scoreboard_entries(self) -> list[ScoreboardEntryDto]:
        return [
            ScoreboardEntryDto(cid, self._room.cid_to_player_data(cid).name, score)
            for (cid, score) in sorted(
                self._scoreboard.items(),
                key=lambda t: t[1], reverse=True
            )
        ]

    def get_full_room_state(self) -> FullGamePlayingStateDto:
        return FullGamePlayingStateDto(
            self.radius,
            [e.to_dto() for e in self._entities.values()],
            self._scoreboard_entries
        )

    def handle_message(self, sender_cid: CID, cmsg: ClientMsg):
        if (eid := get_eid(sender_cid)) in self._entities:
            match cmsg:
                case CSetInputAxesMsg(new_axes):
                    self._entities[eid].input_axes = new_axes
                case CSetBarrelTargetMsg(new_target):
                    self._entities[eid].barrel_target = new_target
                case CShootMsg():
                    self._entities[eid].shoot()
