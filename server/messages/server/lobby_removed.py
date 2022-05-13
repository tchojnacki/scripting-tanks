from typing import Literal
from attr import field, frozen


@frozen
class SLobbyRemovedMsg:
    tag: Literal["s-lobby-removed"] = field(init=False, default="s-removed")
    data: str
