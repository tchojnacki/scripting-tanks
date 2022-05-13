from typing import Literal
from attr import field, frozen
from dto.full_room_state import FullRoomStateDto


@frozen
class FullGameStatePlayerDto:
    cid: str
    name: str


@frozen
class FullGameStateDto(FullRoomStateDto):
    location: Literal["lobby"] = field(init=False, default="lobby")
    name: str
    owner: str
    players: list[FullGameStatePlayerDto]
