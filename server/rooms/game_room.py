import random
from typing import TYPE_CHECKING
from dto.full_game_state import FullGameStateDto, FullGameStatePlayerDto

from dto.new_player import NewPlayerDto
from dto.owner_change import OwnerChangeDto
from dto.player_left import PlayerLeftDto
from .connection_room import ConnectionRoom

if TYPE_CHECKING:
    from room_manager import RoomManager


class GameRoom(ConnectionRoom):
    def __init__(self, room_manager: "RoomManager", owner: str, lid: str, name: str):
        super().__init__(room_manager)
        self._owner: str = owner
        self.lid: str = lid
        self.name: str = name

    def get_full_room_state(self) -> FullGameStateDto:
        return FullGameStateDto(
            self.name,
            self._owner,
            [
                FullGameStatePlayerDto(cid, self._room_manager.cid_to_display_name(cid))
                for cid in self._player_ids
            ]
        )

    async def on_join(self, joiner_cid: str):
        await self.broadcast_message(
            NewPlayerDto(joiner_cid, self._room_manager.cid_to_display_name(joiner_cid))
        )
        await super().on_join(joiner_cid)

    async def on_leave(self, leaver_cid: str):
        await super().on_leave(leaver_cid)
        await self.broadcast_message(PlayerLeftDto(leaver_cid))
        if leaver_cid == self._owner and len(self._player_ids) > 0:
            self._owner = random.choice(tuple(self._player_ids))
            await self.broadcast_message(OwnerChangeDto(self._owner))
