from fastapi import APIRouter, WebSocket
from connection_manager import ConnectionManager

manager = ConnectionManager()
sockets = APIRouter(prefix="/ws")


@sockets.websocket("")
async def game_endpoint(socket: WebSocket):
    await manager.handle_connection(socket)
