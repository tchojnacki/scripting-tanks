from typing import Literal
from attr import field, frozen


@frozen
class SOwnerChangeMsg:
    tag: Literal["s-owner-change"] = field(init=False, default="s-owner-change")
    data: str
