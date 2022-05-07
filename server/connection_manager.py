import asyncio
from fastapi import WebSocket, WebSocketDisconnect
from connection_data import ConnectionData
from rooms.connection_room import ConnectionRoom
from rooms.game_room import GameRoom
from rooms.menu_room import MenuRoom
from utils.display_names import gen_random_name


class ConnectionManager:
    def __init__(self):
        self._menu_room = MenuRoom(self)
        self._game_rooms: dict[str, GameRoom] = {}
        self._active_connections: dict[str, ConnectionData] = {}

    async def _handle_on_connect(self, socket: WebSocket):
        await socket.accept()

        cid = ConnectionData.cid_from_socket(socket)
        con = ConnectionData(socket, self._menu_room, gen_random_name())
        self._active_connections[cid] = con
        await self.send_to_single(cid, "assign-display-name", con.display_name)
        await con.room.on_join(con)

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

    async def return_to_menu(self, cid: str):
        await self._switch_room(cid, self._menu_room)

    async def join_lobby(self, cid: str, lid: str):
        if cid == lid and lid not in self._game_rooms:
            name = f"{self._active_connections[lid].display_name}'s Game"
            self._game_rooms[lid] = GameRoom(self, lid, name)
            await self._menu_room.broadcast_message("new-lobby", {"lid": lid, "name": name})

        if lid in self._game_rooms:
            await self._switch_room(cid, self._game_rooms[lid])

    async def remove_lobby(self, lid: str):
        del self._game_rooms[lid]
        await self._menu_room.broadcast_message("lobby-removed", {"lid": lid})

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
    def lobby_entries(self):
        return self._game_rooms.items()

    def cid_to_name(self, cid: str):
        return self._active_connections[cid].display_name
