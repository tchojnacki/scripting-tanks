from typing import Literal
from attr import field, frozen
from .lobby_data import LobbyDataDto


@frozen
class FullMenuStateDto:
    location: Literal["menu"] = field(init=False, default="menu")
    lobbies: list[LobbyDataDto]