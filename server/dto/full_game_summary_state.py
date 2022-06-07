from typing import Literal
from attrs import field, frozen
from .tank_data import TankDataDto
from .scoreboard_entry import ScoreboardEntryDto


@frozen
class FullGameSummaryStateDto:
    location: Literal["game-summary"] = field(init=False, default="game-summary")
    remaining: int
    tanks: list[TankDataDto]
    scoreboard: list[ScoreboardEntryDto]
