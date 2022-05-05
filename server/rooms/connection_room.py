from abc import ABC, abstractmethod
import asyncio
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from connection_manager import ConnectionManager
    from connection_data import ConnectionData


class ConnectionRoom(ABC):
    def __init__(self, manager: "ConnectionManager"):
        self.manager = manager
        self._player_ids = set()

    async def on_join(self, joiner: "ConnectionData"):
        self._player_ids.add(joiner.cid)
        await self.manager.send_to_single(
            joiner.cid,
            "full-room-state",
            self.get_full_room_state()
        )

    async def on_leave(self, leaver: "ConnectionData"):
        self._player_ids.remove(leaver.cid)

    async def broadcast_message(self, tag: str, data: any):
        await asyncio.gather(*[
            self.manager.send_to_single(cid, tag, data)
            for cid in self._player_ids
        ])

    @property
    def player_count(self):
        return len(self._player_ids)

    @abstractmethod
    def handle_message(self, sender: "ConnectionData", tag: str, data: any):
        pass

    @abstractmethod
    def get_full_room_state(self):
        pass
