from typing import Literal
from attr import field, frozen
from dto.full_room_state import FullRoomStateDto


@frozen
class FullMenuStateLobbyDto:
    lid: str
    name: str
    players: int


@frozen
class FullMenuStateDto(FullRoomStateDto):
    location: Literal["menu"] = field(init=False, default="menu")
    lobbies: list[FullMenuStateLobbyDto]
