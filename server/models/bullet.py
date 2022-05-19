from __future__ import annotations
from typing import TYPE_CHECKING
from math import sin, cos
from attrs import astuple
from dto import BulletDataDto
from utils.uid import CID
from .vector import Vector
from .entity import Entity

if TYPE_CHECKING:
    from rooms.game_states import PlayingGameState

BULLET_SPEED = 1024
RADIUS_GROWTH_TEMPO = 64
MAX_BULLET_RADIUS = 16
GRAVITY_PULL = Vector(0, -200, 0)


class Bullet(Entity):
    def __init__(
        self,
        *,
        world: PlayingGameState,
        owner: CID,
        direction: float,
        pos: Vector
    ):
        super().__init__(
            world=world,
            pos=pos,
            vel=Vector(sin(direction), 0, cos(direction)) * BULLET_SPEED,
            radius=0
        )
        self.owner = owner

    def calculate_forces(self) -> Vector:
        return GRAVITY_PULL * self._mass

    def update(self, dtime: float):
        super().update(dtime)
        self.radius = min(self.radius + dtime * RADIUS_GROWTH_TEMPO, MAX_BULLET_RADIUS)

    def to_dto(self) -> BulletDataDto:
        return BulletDataDto(
            self.eid, self.owner, astuple(self.pos), self.radius
        )
