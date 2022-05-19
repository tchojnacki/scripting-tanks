from __future__ import annotations
from typing import TYPE_CHECKING
from math import sin, cos
from attrs import astuple
from dto import BulletDataDto
from .vector import Vector
from .entity import Entity

if TYPE_CHECKING:
    from rooms.game_states import PlayingGameState

BULLET_SPEED = 1024
BULLET_GROW_TEMPO = 32
GRAVITY_PULL = Vector(0, -200, 0)


class Bullet(Entity):
    def __init__(
        self,
        *,
        world: PlayingGameState,
        direction: float,
        pos: Vector
    ):
        super().__init__(
            world=world,
            pos=pos,
            vel=Vector(sin(direction), 0, cos(direction)) * BULLET_SPEED,
            size=Vector(32, 32, 32),
        )
        self._visible_radius = 0

    def calculate_forces(self) -> Vector:
        return GRAVITY_PULL * self._mass

    def update(self, dtime: float):
        super().update(dtime)
        self._visible_radius = min(self._visible_radius + dtime * BULLET_GROW_TEMPO, self._size.y/2)

    def to_dto(self) -> BulletDataDto:
        return BulletDataDto(
            self.eid, astuple(self._pos), self._visible_radius
        )
