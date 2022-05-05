import asyncio
from typing import TYPE_CHECKING
from connection_data import ConnectionData
from .connection_room import ConnectionRoom

if TYPE_CHECKING:
    from connection_manager import ConnectionManager


class GameRoom(ConnectionRoom):
    def __init__(self, manager: "ConnectionManager", lid):
        super().__init__(manager)
        self.lid = lid

    def get_full_room_state(self):
        return {
            "location": "lobby",
            "players": list(self._player_ids),
        }

    async def on_join(self, joiner: "ConnectionData"):
        await self.broadcast_message("new-player", joiner.cid)
        await super().on_join(joiner)

    async def on_leave(self, leaver: "ConnectionData"):
        await super().on_leave(leaver)
        await self.broadcast_message("player-left", leaver.cid)

    def handle_message(self, sender: ConnectionData, tag: str, data: any):
        if tag == "leave-lobby":
            asyncio.ensure_future(self.manager.return_to_menu(sender.cid))
