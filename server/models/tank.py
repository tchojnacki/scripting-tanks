from typing import Optional
from attr import astuple
from dto import EntityDataDto
from utils.uid import CID, get_eid
from .vector import Vector
from .entity import Entity


class Tank(Entity):
    def __init__(
        self,
        *,
        cid: CID,
        pos: Optional[Vector] = None,
        color: str,
        pitch: float = 0
    ):
        super().__init__(eid=get_eid(cid), kind="tank", pos=pos)
        self.cid = cid
        self.color = color
        self.pitch = pitch
        self.omega: float = 0

    def to_dto(self) -> EntityDataDto:
        return EntityDataDto(
            self.eid, self.cid, self.kind,
            self.color, astuple(self.pos), self.pitch
        )
