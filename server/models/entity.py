from typing import Literal
from attr import define, astuple
from dto import EntityDataDto
from utils.uid import CID, EID
from .vector import Vector


@define
class Entity:
    eid: EID
    cid: CID
    kind: Literal["tank"]
    color: str
    pos: Vector
    pitch: float

    def update(self):
        pass

    def to_dto(self) -> EntityDataDto:
        return EntityDataDto(
            self.eid, self.cid, self.kind,
            self.color, astuple(self.pos), self.pitch
        )
