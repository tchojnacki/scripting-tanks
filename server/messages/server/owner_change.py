from typing import Literal
from attr import field, frozen
from utils.uid import CID


@frozen
class SOwnerChangeMsg:
    tag: Literal["s-owner-change"] = field(init=False, default="s-owner-change")
    data: CID