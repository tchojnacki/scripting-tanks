from dataclasses import dataclass
from fastapi import WebSocket
from utils.uid import get_uid


@dataclass
class ConnectionData:
    socket: WebSocket
    display_name: str

    @staticmethod
    def cid_from_socket(socket: WebSocket) -> str:
        return "cid$" + get_uid(id(socket))

    @property
    def cid(self) -> str:
        return ConnectionData.cid_from_socket(self.socket)
