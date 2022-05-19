from __future__ import annotations
from abc import ABC, abstractmethod
from typing import Optional, TYPE_CHECKING
from dto import EntityDataDto
from utils.uid import EID, get_eid
from .vector import Vector

if TYPE_CHECKING:
    from rooms.game_states import PlayingGameState


SEA_HEIGHT = -100


def _optional_vec(v: Vector | None) -> Vector:
    return v if v is not None else Vector.zero()


class Entity(ABC):
    def __init__(
        self,
        *,
        world: PlayingGameState,
        eid: Optional[EID] = None,
        pos: Optional[Vector] = None,
        vel: Optional[Vector] = None,
        radius: float = 0,
        mass: float = 1,
    ):
        self.world = world
        self.eid = eid if eid is not None else get_eid()
        self.pos = _optional_vec(pos)
        self._vel = _optional_vec(vel)
        self._acc = Vector.zero()
        self.radius = radius
        self._mass = mass

    def update(self, dtime: float):
        forces = self.calculate_forces()
        self._acc = forces / self._mass
        self._vel += self._acc * dtime
        self.pos += self._vel * dtime

        if self.pos.y + self.radius < SEA_HEIGHT:
            self.world.destroy(self)

    def calculate_forces(self) -> Vector:
        return Vector.zero()

    @property
    def highest_point(self) -> float:
        return self.pos.y + self.radius

    @abstractmethod
    def to_dto(self) -> EntityDataDto:
        pass
