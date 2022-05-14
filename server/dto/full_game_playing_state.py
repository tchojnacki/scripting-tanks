from typing import Literal
from attr import field, frozen


@frozen
class FullGamePlayingStateDto:
    location: Literal["game-playing"] = field(init=False, default="game-playing")
