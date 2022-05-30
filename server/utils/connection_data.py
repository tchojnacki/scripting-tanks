from attrs import define, field, setters
from fastapi import WebSocket
from dto import PlayerDataDto
from utils.color import assign_color
from utils.tank_colors import TankColors
from utils.uid import CID, get_cid


@define
class ConnectionData:
    socket: WebSocket = field(on_setattr=setters.frozen)
    display_name: str
    colors: TankColors = field(init=False)

    @colors.default
    def _default_colors(self):
        c = assign_color(self.cid)
        return (c, c)

    @staticmethod
    def cid_from_socket(socket: WebSocket) -> CID:
        return get_cid(id(socket))

    @property
    def cid(self) -> CID:
        return ConnectionData.cid_from_socket(self.socket)

    @property
    def player_data(self) -> PlayerDataDto:
        return PlayerDataDto(self.cid, self.display_name, self.colors)
