import asyncio
from fastapi import WebSocket, WebSocketDisconnect
from connection_data import ConnectionData
from room_manager import RoomManager
from utils.display_names import gen_random_name


class ConnectionManager:
    def __init__(self):
        self._active_connections: dict[str, ConnectionData] = {}
        self._room_manager: RoomManager = RoomManager(self)

    async def _handle_on_connect(self, socket: WebSocket):
        await socket.accept()

        cid = ConnectionData.cid_from_socket(socket)
        con = ConnectionData(socket, gen_random_name())
        self._active_connections[cid] = con
        await self.send_to_single(cid, "assign-display-name", con.display_name)
        await self._room_manager.on_connect(cid)

    def _handle_on_disconnect(self, socket: WebSocket):
        cid = ConnectionData.cid_from_socket(socket)
        del self._active_connections[cid]
        asyncio.ensure_future(self._room_manager.on_disconnect(cid))

    def _handle_message(self, sender: ConnectionData, tag: str, data: any):
        self._room_manager.handle_message(sender.cid, tag, data)

    async def handle_connection(self, socket: WebSocket):
        await self._handle_on_connect(socket)
        cid = ConnectionData.cid_from_socket(socket)
        connection = self._active_connections[cid]
        try:
            while True:
                message = await socket.receive_json()
                if "tag" in message:
                    self._handle_message(connection, message["tag"], message["data"])
        except WebSocketDisconnect:
            self._handle_on_disconnect(socket)

    async def send_to_single(self, cid: str, tag: str, data: any):
        await self._active_connections[cid].socket.send_json({"tag": tag, "data": data})

    def cid_to_display_name(self, cid: str) -> str:
        return self._active_connections[cid].display_name
