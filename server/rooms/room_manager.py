import asyncio
from typing import Optional, TYPE_CHECKING
from dto.lobby_data import LobbyDataDto
from messages.client import ClientMsg, CCreateLobbyMsg, CEnterLobbyMsg, CLeaveLobbyMsg
from messages.server import SLobbyRemovedMsg, ServerMsg, SNewLobbyMsg
from rooms import ConnectionRoom, GameRoom, MenuRoom
from utils.uid import CID, LID, get_lid

if TYPE_CHECKING:
    from connection_manager import ConnectionManager


class RoomManager:
    def __init__(self, connection_manager: "ConnectionManager"):
        self._connection_manager = connection_manager
        self._menu_room = MenuRoom(self)
        self._game_rooms: dict[LID, GameRoom] = {}

    def _connection_room(self, cid: CID) -> Optional[ConnectionRoom]:
        return next((
            room for room in [self._menu_room, *self._game_rooms.values()]
            if room.has_player(cid)
        ), None)

    async def on_connect(self, cid: CID):
        await self._switch_room(cid, self._menu_room)

    async def on_disconnect(self, cid: CID):
        await self._switch_room(cid, None)

    def handle_message(self, sender_cid: CID, cmsg: ClientMsg):
        match cmsg:
            case CCreateLobbyMsg():
                asyncio.ensure_future(self._create_lobby(sender_cid))
            case CEnterLobbyMsg(lid):
                asyncio.ensure_future(self._join_game_room(sender_cid, lid))
            case CLeaveLobbyMsg():
                asyncio.ensure_future(self._switch_room(sender_cid, self._menu_room))
            case _:
                self._connection_room(sender_cid).handle_message(sender_cid, cmsg)

    def cid_to_display_name(self, cid: CID) -> str:
        return self._connection_manager.cid_to_display_name(cid)

    async def send_to_single(self, cid: CID, smsg: ServerMsg):
        await self._connection_manager.send_to_single(cid, smsg)

    async def _switch_room(self, cid: CID, new_room: Optional[ConnectionRoom]):
        prev_room = self._connection_room(cid)
        if prev_room is not None:
            await prev_room.on_leave(cid)
            if isinstance(prev_room, GameRoom) and prev_room.player_count == 0:
                await self._remove_game_room(prev_room.lid)

        if new_room is not None:
            await new_room.on_join(cid)

    async def _remove_game_room(self, lid: LID):
        del self._game_rooms[lid]
        await self._menu_room.broadcast_message(SLobbyRemovedMsg(lid))

    async def _create_lobby(self, owner_cid: CID):
        lid = get_lid()
        name = f"{self._connection_manager.cid_to_display_name(owner_cid)}'s Game"
        self._game_rooms[lid] = GameRoom(self, owner_cid, lid, name)
        await self._menu_room.broadcast_message(SNewLobbyMsg(LobbyDataDto(lid, name, 1)))
        await self._join_game_room(owner_cid, lid)

    async def _join_game_room(self, cid: CID, lid: LID):
        if lid in self._game_rooms:
            await self._switch_room(cid, self._game_rooms[lid])

    @property
    def lobby_entries(self) -> list[LobbyDataDto]:
        return [
            LobbyDataDto(room.lid, room.name, room.player_count)
            for room in self._game_rooms.values()
        ]