import asyncio
from attrs import asdict
from fastapi import WebSocket, WebSocketDisconnect
from dto import PlayerDataDto
from messages.client import ClientMsg, parse_message, CRerollNameMsg, CCustomizeColorsMsg
from messages.server import SAssignIdentityMsg, ServerMsg
from rooms.room_manager import RoomManager
from utils.color import assign_color
from utils.display_names import gen_random_name
from utils.connection_data import ConnectionData
from utils.uid import CID, get_cid


class ConnectionManager:
    def __init__(self):
        self._active_connections: dict[CID, ConnectionData] = {}
        self._bots: set[CID] = set()
        self._room_manager: RoomManager = RoomManager(self)

    async def _handle_on_connect(self, socket: WebSocket):
        await socket.accept()

        cid = ConnectionData.cid_from_socket(socket)
        con = ConnectionData(socket, gen_random_name())
        self._active_connections[cid] = con
        await self.send_to_single(cid, SAssignIdentityMsg(con.player_data))
        await self._room_manager.on_connect(cid)

    async def add_bot(self) -> CID:
        cid = get_cid()
        self._bots.add(cid)
        await self._room_manager.on_connect(cid)
        return cid

    def _handle_on_disconnect(self, socket: WebSocket):
        cid = ConnectionData.cid_from_socket(socket)
        del self._active_connections[cid]
        asyncio.ensure_future(self._room_manager.on_disconnect(cid))

    def _handle_message(self, sender_cid: CID, cmsg: ClientMsg):
        con = self._active_connections[sender_cid]
        match cmsg:
            case CRerollNameMsg() if self._room_manager.can_customize(sender_cid):
                con.display_name = gen_random_name()
                asyncio.ensure_future(self.send_to_single(
                    sender_cid, SAssignIdentityMsg(con.player_data)
                ))
            case CCustomizeColorsMsg(dto) if self._room_manager.can_customize(sender_cid):
                con.colors = dto.colors
                asyncio.ensure_future(self.send_to_single(
                    sender_cid, SAssignIdentityMsg(con.player_data)
                ))
            case _:
                self._room_manager.handle_message(sender_cid, cmsg)

    async def handle_connection(self, socket: WebSocket):
        await self._handle_on_connect(socket)
        cid = ConnectionData.cid_from_socket(socket)
        try:
            while True:
                message = await socket.receive_json()
                if (parsed_msg := parse_message(message)) is not None:
                    self._handle_message(cid, parsed_msg)
        except WebSocketDisconnect:
            self._handle_on_disconnect(socket)

    async def send_to_single(self, cid: CID, smsg: ServerMsg):
        if cid not in self._bots:
            await self._active_connections[cid].socket.send_json(asdict(smsg))

    def cid_to_player_data(self, cid: CID) -> PlayerDataDto:
        if cid in self._bots:
            c = assign_color(cid)
            return PlayerDataDto(cid, "BOT", (c, c), True)

        return self._active_connections[cid].player_data
