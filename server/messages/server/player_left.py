from typing import Literal
from attr import field, frozen
from utils.uid import CID


@frozen
class SPlayerLeftMsg:
    tag: Literal["s-player-left"] = field(init=False, default="s-player-left")
    data: CID
