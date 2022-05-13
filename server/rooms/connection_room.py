from abc import ABC, abstractmethod
import asyncio
from typing import TYPE_CHECKING

from dto.tagged_dto import TaggedDto
from dto.full_room_state import FullRoomStateDto

if TYPE_CHECKING:
    from room_manager import RoomManager


class ConnectionRoom(ABC):
    def __init__(self, room_manager: "RoomManager"):
        self._room_manager = room_manager
        self._player_ids: set[str] = set()

    async def on_join(self, joiner_cid: str):
        self._player_ids.add(joiner_cid)
        await self._room_manager.send_to_single(
            joiner_cid,
            self.get_full_room_state()
        )

    async def on_leave(self, leaver_cid: str):
        self._player_ids.remove(leaver_cid)

    async def broadcast_message(self, dto: TaggedDto):
        await asyncio.gather(*[
            self._room_manager.send_to_single(cid, dto)
            for cid in self._player_ids
        ])

    def has_player(self, cid: str) -> bool:
        return cid in self._player_ids

    @property
    def player_count(self) -> int:
        return len(self._player_ids)

    def handle_message(self, sender_cid: str, tag: str, data: any):
        pass

    @abstractmethod
    def get_full_room_state(self) -> FullRoomStateDto:
        pass
