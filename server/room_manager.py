import asyncio
from typing import Optional
from rooms.connection_room import ConnectionRoom
from rooms.game_room import GameRoom
from rooms.menu_room import MenuRoom
from utils.uid import get_uid


class RoomManager:
    def __init__(self, connection_manager):
        self._connection_manager = connection_manager
        self._menu_room = MenuRoom(self)
        self._game_rooms: dict[str, GameRoom] = {}

    def _connection_room(self, cid: str) -> Optional[ConnectionRoom]:
        return next((
            room for room in [self._menu_room, *self._game_rooms.values()]
            if room.has_player(cid)
        ), None)

    async def on_connect(self, cid: str):
        await self._switch_room(cid, self._menu_room)

    async def on_disconnect(self, cid: str):
        await self._switch_room(cid, None)

    def handle_message(self, sender_cid: str, tag: str, data: any):
        match tag:
            case "create-lobby":
                asyncio.ensure_future(self._create_lobby(sender_cid))
            case "enter-lobby":
                asyncio.ensure_future(self._join_game_room(sender_cid, data))
            case "leave-lobby":
                asyncio.ensure_future(self._switch_room(sender_cid, self._menu_room))
            case _:
                self._connection_room(sender_cid).handle_message(sender_cid, tag, data)

    def cid_to_display_name(self, cid: str) -> str:
        return self._connection_manager.cid_to_display_name(cid)

    async def send_to_single(self, cid: str, tag: str, data: any):
        await self._connection_manager.send_to_single(cid, tag, data)

    async def _switch_room(self, cid: str, new_room: Optional[ConnectionRoom]):
        prev_room = self._connection_room(cid)
        if prev_room is not None:
            await prev_room.on_leave(cid)
            if isinstance(prev_room, GameRoom) and prev_room.player_count == 0:
                await self._remove_game_room(prev_room.lid)

        if new_room is not None:
            await new_room.on_join(cid)

    async def _remove_game_room(self, lid: str):
        del self._game_rooms[lid]
        await self._menu_room.broadcast_message("lobby-removed", {"lid": lid})

    async def _create_lobby(self, owner_cid: str):
        lid = "lid$" + get_uid()
        name = f"{self._connection_manager.cid_to_display_name(owner_cid)}'s Game"
        self._game_rooms[lid] = GameRoom(self, owner_cid, lid, name)
        await self._menu_room.broadcast_message(
            "new-lobby",
            {"lid": lid, "name": name, "players": 1}
        )
        await self._join_game_room(owner_cid, lid)

    async def _join_game_room(self, cid: str, lid: str):
        if lid in self._game_rooms:
            await self._switch_room(cid, self._game_rooms[lid])

    @property
    def lobby_entries(self) -> list[dict[str, str]]:
        return [
            {"lid": room.lid, "name": room.name, "players": room.player_count}
            for room in self._game_rooms.values()
        ]
