from typing import Literal
from attr import field, frozen
from utils.uid import LID


@frozen
class SLobbyRemovedMsg:
    tag: Literal["s-lobby-removed"] = field(init=False, default="s-removed")
    data: LID
