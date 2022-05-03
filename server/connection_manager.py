import asyncio
from fastapi import WebSocket, WebSocketDisconnect


class ConnectionManager:
    def __init__(self):
        self._active_connections: dict[int, WebSocket] = {}
        self._connect_handlers = []
        self._disconnect_handlers = []

    async def _handle_on_connect(self, socket: WebSocket):
        await socket.accept()
        socket_id = id(socket)
        self._active_connections[socket_id] = socket

        for handler in self._connect_handlers:
            asyncio.ensure_future(handler(socket_id))

    def _handle_on_disconnect(self, socket: WebSocket):
        socket_id = id(socket)
        del self._active_connections[socket_id]

        for handler in self._disconnect_handlers:
            asyncio.ensure_future(handler(socket_id))

    def on_connect(self, func):
        self._connect_handlers.append(func)
        return func

    def on_disconnect(self, func):
        self._disconnect_handlers.append(func)
        return func

    async def handle_connection(self, socket: WebSocket):
        await self._handle_on_connect(socket)
        try:
            while True:
                data = await socket.receive_json()
                print(data)
        except WebSocketDisconnect:
            self._handle_on_disconnect(socket)

    async def send_to_single(self, socket_id: int, tag: str, data: any):
        await self._active_connections[socket_id].send_json({"tag": tag, "data": data})

    async def send_to_all(self, tag: str, data: any):
        await asyncio.gather(*[
            self.send_to_single(socket_id, tag, data)
            for socket_id in self._active_connections
        ])

    def all_socket_ids(self):
        return list(self._active_connections.keys())
