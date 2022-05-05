import asyncio
from fastapi import WebSocket, WebSocketDisconnect
from connection_data import ConnectionData
from rooms.connection_room import ConnectionRoom
from rooms.game_room import GameRoom
from rooms.menu_room import MenuRoom


class ConnectionManager:
    def __init__(self):
        self._menu_room = MenuRoom(self)
        self._game_rooms: list[GameRoom] = []
        self._active_connections: dict[str, ConnectionData] = {}

    async def _handle_on_connect(self, socket: WebSocket):
        await socket.accept()

        cid = ConnectionData.cid_from_socket(socket)
        con = ConnectionData(socket, self._menu_room)
        self._active_connections[cid] = con
        asyncio.ensure_future(con.room.on_join(con))

    def _handle_on_disconnect(self, socket: WebSocket):
        cid = ConnectionData.cid_from_socket(socket)
        con = self._active_connections[cid]
        del self._active_connections[cid]
        asyncio.ensure_future(con.room.on_leave(con))

    async def _switch_room(self, cid: str, new_room: ConnectionRoom):
        con = self._active_connections[cid]
        await con.room.on_leave(con)
        con.room = new_room
        await con.room.on_join(con)

    def return_to_menu(self, cid: str):
        asyncio.ensure_future(self._switch_room(cid, self._menu_room))

    def join_lobby(self, cid: str, lobby_name: int):
        asyncio.ensure_future(self._switch_room(cid, self._game_rooms[lobby_name]))

    def _handle_message(self, sender: ConnectionData, tag: str, data: any):
        sender.room.handle_message(sender, tag, data)

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

    @property
    def lobby_list(self):
        return self._game_rooms

    def create_lobby(self):
        self._game_rooms.append(GameRoom(self))
        return len(self._game_rooms) - 1
