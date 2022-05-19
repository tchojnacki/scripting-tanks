from typing import ClassVar, Literal
from attr import frozen


@frozen
class CSetBarrelTargetMsg:
    tag: ClassVar[Literal["c-set-barrel-target"]] = "c-set-barrel-target"
    data: float
