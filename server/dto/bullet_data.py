from typing import Literal, Tuple
from attrs import field, frozen
from utils.uid import CID, EID


@frozen
class BulletDataDto:
    kind: Literal["bullet"] = field(init=False, default="bullet")
    eid: EID
    owner: CID
    pos: Tuple[float, float, float]
    radius: float
