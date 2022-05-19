from typing import Literal
from attrs import field, frozen
from dto import PlayerDataDto


@frozen
class SNewPlayerMsg:
    tag: Literal["s-new-player"] = field(init=False, default="s-new-player")
    data: PlayerDataDto
