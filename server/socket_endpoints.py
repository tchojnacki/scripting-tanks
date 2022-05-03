from fastapi import APIRouter, WebSocket
from connection_manager import ConnectionManager

manager = ConnectionManager()
sockets = APIRouter(prefix="/ws")


@manager.on_connect
async def on_connect(socket_id):
    await manager.send_to_all("connected", socket_id)
    await manager.send_to_single(socket_id, "full_state", manager.all_socket_ids())


@manager.on_disconnect
async def on_disconnect(socket_id):
    await manager.send_to_all("disconnected", socket_id)


@sockets.websocket("/game")
async def websocket_endpoint(websocket: WebSocket):
    await manager.handle_connection(websocket)
