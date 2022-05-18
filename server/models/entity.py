from abc import ABC, abstractmethod
from typing import Literal, Optional
from dto import EntityDataDto
from utils.uid import EID, get_eid
from .vector import Vector


class Entity(ABC):
    def __init__(
        self,
        *,
        eid: Optional[EID] = None,
        kind: Literal["tank"],
        pos: Optional[Vector] = None,
        vel: Optional[Vector] = None,
        acc: Optional[Vector] = None,
    ):
        self.eid = eid if eid is not None else get_eid()
        self.kind = kind
        self.pos = pos if pos is not None else Vector.zero()
        self.vel = vel if vel is not None else Vector.zero()
        self.acc = acc if acc is not None else Vector.zero()

    def update(self):
        pass

    @abstractmethod
    def to_dto(self) -> EntityDataDto:
        pass
