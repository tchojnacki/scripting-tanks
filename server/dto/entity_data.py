from typing import Literal
from attr import frozen
from utils.uid import CID, EID


@frozen
class EntityDataDto:
    eid: EID
    cid: CID
    kind: Literal["tank"]
    color: str
    x: float
    z: float
    pitch: float
