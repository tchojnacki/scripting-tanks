from typing import Literal
from attrs import field, frozen
from dto import LobbyDataDto


@frozen
class SUpsertLobbyMsg:
    tag: Literal["s-upsert-lobby"] = field(init=False, default="s-upsert-lobby")
    data: LobbyDataDto
