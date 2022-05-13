from typing import Literal
from attr import field, frozen
from dto.lobby_data import LobbyDataDto


@frozen
class SNewLobbyMsg:
    tag: Literal["s-new-lobby"] = field(init=False, default="s-new-lobby")
    data: LobbyDataDto
