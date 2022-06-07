from typing import Literal
from attrs import field, frozen
from .scoreboard_entry import ScoreboardEntryDto
from .entity_data import EntityDataDto


@frozen
class FullGamePlayingStateDto:
    location: Literal["game-playing"] = field(init=False, default="game-playing")
    radius: int
    entities: list[EntityDataDto]
    scoreboard: list[ScoreboardEntryDto]
