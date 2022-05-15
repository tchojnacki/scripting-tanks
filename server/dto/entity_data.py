from typing import Literal
from attr import frozen
from utils.uid import EID


@frozen
class EntityDataDto:
    eid: EID
    kind: Literal["tank"]
    color: str
    x: float
    z: float
    angle: float
