from abc import ABC, abstractmethod
import asyncio
from typing import TYPE_CHECKING
from dto import FullRoomStateDto
from messages.server import ServerMsg, SFullRoomStateMsg
from utils.uid import CID

if TYPE_CHECKING:
    from room_manager import RoomManager


class ConnectionRoom(ABC):
    def __init__(self, room_manager: "RoomManager"):
        self._room_manager = room_manager
        self._player_ids: set[CID] = set()

    async def on_join(self, joiner_cid: CID):
        self._player_ids.add(joiner_cid)
        await self._room_manager.send_to_single(
            joiner_cid,
            SFullRoomStateMsg(self.get_full_room_state()),
        )

    async def on_leave(self, leaver_cid: CID):
        self._player_ids.remove(leaver_cid)

    async def broadcast_message(self, smsg: ServerMsg):
        await asyncio.gather(*[
            self._room_manager.send_to_single(cid, smsg)
            for cid in self._player_ids
        ])

    def has_player(self, cid: CID) -> bool:
        return cid in self._player_ids

    @property
    def player_count(self) -> int:
        return len(self._player_ids)

    def handle_message(self, sender_cid: CID, tag: str, data: any):
        pass

    @abstractmethod
    def get_full_room_state(self) -> FullRoomStateDto:
        pass
