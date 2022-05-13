from dataclasses import dataclass
from fastapi import WebSocket
from utils.uid import CID, get_cid


@dataclass
class ConnectionData:
    socket: WebSocket
    display_name: str

    @staticmethod
    def cid_from_socket(socket: WebSocket) -> CID:
        return get_cid(id(socket))

    @property
    def cid(self) -> CID:
        return ConnectionData.cid_from_socket(self.socket)
