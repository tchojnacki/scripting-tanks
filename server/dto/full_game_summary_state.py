from typing import Literal
from attrs import field, frozen
from .tank_data import TankDataDto


@frozen
class FullGameSummaryStateDto:
    location: Literal["game-summary"] = field(init=False, default="game-summary")
    remaining: int
    tanks: list[TankDataDto]
