from typing import Literal
from attr import field, frozen
from dto.player_data import PlayerDataDto


@frozen
class SNewPlayerMsg:
    tag: Literal["s-new-player"] = field(init=False, default="s-new-player")
    data: PlayerDataDto
