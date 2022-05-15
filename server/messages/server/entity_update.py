from typing import Literal
from attr import field, frozen
from dto import EntityDataDto


@frozen
class SEntityUpdateMsg:
    tag: Literal["s-entity-update"] = field(init=False, default="s-entity-update")
    data: EntityDataDto
