from typing import Literal
from attr import field, frozen
from dto.full_room_state import FullRoomStateDto


@frozen
class SFullRoomStateMsg:
    tag: Literal["s-full-room-state"] = field(init=False, default="s-full-room-state")
    data: FullRoomStateDto