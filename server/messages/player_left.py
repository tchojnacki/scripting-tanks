from typing import Literal
from attr import field, frozen


@frozen
class SPlayerLeftMsg:
    tag: Literal["s-player-left"] = field(init=False, default="s-player-left")
    data: str
