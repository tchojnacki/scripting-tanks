from dataclasses import dataclass
from hashlib import md5
from fastapi import WebSocket


@dataclass
class ConnectionData:
    socket: WebSocket
    display_name: str

    @staticmethod
    def cid_from_socket(socket: WebSocket) -> str:
        return md5(str(id(socket)).encode()).hexdigest()

    @property
    def cid(self) -> str:
        return ConnectionData.cid_from_socket(self.socket)
