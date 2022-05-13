import asyncio
from attr import asdict
from cattrs import unstructure
from fastapi import WebSocket, WebSocketDisconnect
from connection_data import ConnectionData
from messages.client import ClientMsg, parse_message, CRefetchDisplayNameMsg
from messages.server import SAssignDisplayNameMsg, ServerMsg
from room_manager import RoomManager
from utils.display_names import gen_random_name
from utils.uid import CID


class ConnectionManager:
    def __init__(self):
        self._active_connections: dict[CID, ConnectionData] = {}
        self._room_manager: RoomManager = RoomManager(self)

    async def _handle_on_connect(self, socket: WebSocket):
        await socket.accept()

        cid = ConnectionData.cid_from_socket(socket)
        con = ConnectionData(socket, gen_random_name())
        self._active_connections[cid] = con
        await self.send_to_single(cid, SAssignDisplayNameMsg(con.display_name))
        await self._room_manager.on_connect(cid)

    def _handle_on_disconnect(self, socket: WebSocket):
        cid = ConnectionData.cid_from_socket(socket)
        del self._active_connections[cid]
        asyncio.ensure_future(self._room_manager.on_disconnect(cid))

    def _handle_message(self, sender_cid: CID, cmsg: ClientMsg):
        match cmsg:
            case CRefetchDisplayNameMsg():
                asyncio.ensure_future(self.send_to_single(
                    sender_cid,
                    SAssignDisplayNameMsg(self._active_connections[sender_cid].display_name)
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
        await self._active_connections[cid].socket.send_json(asdict(smsg))

    def cid_to_display_name(self, cid: CID) -> str:
        return self._active_connections[cid].display_name
