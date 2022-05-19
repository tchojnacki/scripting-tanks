from typing import Literal
from attrs import field, frozen
from utils.uid import CID
from .player_data import PlayerDataDto


@frozen
class FullGameWaitingStateDto:
    location: Literal["game-waiting"] = field(init=False, default="game-waiting")
    name: str
    owner: CID
    players: list[PlayerDataDto]
