from typing import Literal, Tuple
from attrs import field, frozen
from utils.uid import EID


@frozen
class BulletDataDto:
    kind: Literal["bullet"] = field(init=False, default="bullet")
    eid: EID
    pos: Tuple[float, float, float]
    radius: float
