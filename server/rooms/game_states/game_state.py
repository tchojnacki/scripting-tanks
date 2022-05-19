from __future__ import annotations
from typing import TYPE_CHECKING
from abc import ABC, abstractmethod
from dto import FullGameStateDto
from messages.client import ClientMsg
from utils.uid import CID

if TYPE_CHECKING:
    from rooms.game_room import GameRoom


class GameState(ABC):
    def __init__(self, room: GameRoom):
        self._room = room

    @abstractmethod
    def get_full_room_state(self) -> FullGameStateDto:
        pass

    def handle_message(self, sender_cid: CID, cmsg: ClientMsg):
        pass

    async def on_join(self, joiner_cid: CID):
        pass

    async def on_leave(self, leaver_cid: CID):
        pass
