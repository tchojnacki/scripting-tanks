from typing import Literal
from attr import field, frozen


@frozen
class SAssignDisplayNameMsg:
    tag: Literal["s-assign-display-name"] = field(init=False, default="s-assign-display-name")
    data: str
