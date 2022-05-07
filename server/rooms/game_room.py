from typing import TYPE_CHECKING
from .connection_room import ConnectionRoom

if TYPE_CHECKING:
    from room_manager import RoomManager


class GameRoom(ConnectionRoom):
    def __init__(self, room_manager: "RoomManager", lid: str, name: str):
        super().__init__(room_manager)
        self.lid = lid
        self.name = name

    def get_full_room_state(self) -> any:
        return {
            "location": "lobby",
            "name": self.name,
            "players": list(map(
                lambda cid: {"cid": cid, "name": self._room_manager.cid_to_display_name(cid)},
                self._player_ids
            )),
        }

    async def on_join(self, joiner_cid: str):
        await self.broadcast_message(
            "new-player",
            {"cid": joiner_cid, "name": self._room_manager.cid_to_display_name(joiner_cid)}
        )
        await super().on_join(joiner_cid)

    async def on_leave(self, leaver_cid: str):
        await super().on_leave(leaver_cid)
        await self.broadcast_message("player-left", leaver_cid)
