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
        mass: float = 1
    ):
        self.eid = eid if eid is not None else get_eid()
        self._kind = kind
        self._pos = pos if pos is not None else Vector.zero()
        self._vel = vel if vel is not None else Vector.zero()
        self._acc = acc if acc is not None else Vector.zero()
        self._mass = mass

    def update(self, dtime: float):
        forces = self.calculate_forces()
        self._acc = forces / self._mass
        self._vel += self._acc * dtime
        self._pos += self._vel * dtime

    def calculate_forces(self) -> Vector:
        return Vector.zero()

    @abstractmethod
    def to_dto(self) -> EntityDataDto:
        pass
