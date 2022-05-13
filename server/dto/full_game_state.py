from typing import Literal
from attr import field, frozen
from .player_data import PlayerDataDto


@frozen
class FullGameStateDto:
    location: Literal["lobby"] = field(init=False, default="lobby")
    name: str
    owner: str
    players: list[PlayerDataDto]
