from typing import Literal
from attr import field, frozen
from dto.lobby_data import LobbyDataDto


@frozen
class SUpsertLobbyMsg:
    tag: Literal["s-upsert-lobby"] = field(init=False, default="s-upsert-lobby")
    data: LobbyDataDto
