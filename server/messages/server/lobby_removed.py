from typing import Literal
from attrs import field, frozen
from utils.uid import LID


@frozen
class SLobbyRemovedMsg:
    tag: Literal["s-lobby-removed"] = field(init=False, default="s-lobby-removed")
    data: LID
