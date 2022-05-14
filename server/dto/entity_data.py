from attr import frozen
from utils.uid import EID


@frozen
class EntityDataDto:
    eid: EID
    color: str
    x: float
    y: float
