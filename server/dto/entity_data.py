from typing import Literal, Tuple
from attr import frozen
from utils.uid import CID, EID


@frozen
class EntityDataDto:
    eid: EID
    cid: CID
    kind: Literal["tank"]
    color: str
    pos: Tuple[float, float, float]
    pitch: float
