from dataclasses import dataclass
from hashlib import md5
from fastapi import WebSocket
from rooms.connection_room import ConnectionRoom


@dataclass
class ConnectionData:
    socket: WebSocket
    room: ConnectionRoom

    @staticmethod
    def cid_from_socket(socket: WebSocket) -> str:
        return md5(str(id(socket)).encode()).hexdigest()

    @property
    def cid(self) -> str:
        return ConnectionData.cid_from_socket(self.socket)
