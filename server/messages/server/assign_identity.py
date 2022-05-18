from typing import Literal
from attr import field, frozen
from dto import PlayerDataDto


@frozen
class SAssignIdentityMsg:
    tag: Literal["s-assign-identity"] = field(init=False, default="s-assign-identity")
    data: PlayerDataDto