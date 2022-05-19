from __future__ import annotations
from math import cos, sin, pi, copysign
from typing import TYPE_CHECKING, Optional
from attr import astuple
from dto import EntityDataDto, InputAxesDto
from utils.uid import CID, get_eid
from .vector import Vector
from .entity import Entity

if TYPE_CHECKING:
    from rooms.game_states import PlayingGameState


C_DRAG = 100
C_ROLL_RESIST = 5_000
ENGINE_FORCE = 2_000_000
REVERSE_MULT = 0.25
TANK_MASS = 10_000
TURN_DEGREE = pi/12

GRAVITY_PULL = Vector(0, -600, 0)


class Tank(Entity):
    def __init__(
        self,
        *,
        world: PlayingGameState,
        cid: CID,
        pos: Optional[Vector] = None,
        color: str,
        pitch: float = 0
    ):
        super().__init__(
            world=world,
            eid=get_eid(cid),
            kind="tank",
            pos=pos,
            mass=TANK_MASS,
            size=Vector(96, 64, 96)
        )
        self._cid = cid
        self._color = color
        self._pitch = pitch
        self.input_axes = InputAxesDto(0, 0)

    def calculate_forces(self) -> Vector:
        u = Vector(sin(self._pitch), 0, cos(self._pitch))

        engine_force = (1 if Vector.dot(self._vel, u) > 0
                        else REVERSE_MULT) * ENGINE_FORCE * self.input_axes.vertical

        f_traction = u * engine_force
        f_drag = -C_DRAG * self._vel * self._vel.length
        f_roll_resist = -C_ROLL_RESIST * self._vel
        f_gravity = self._calculate_gravity_force()

        return f_traction + f_drag + f_roll_resist + f_gravity

    def _calculate_gravity_force(self) -> Vector:
        f_gravity = Vector.zero()

        if self._pos.length > self.world.radius:
            f_gravity += GRAVITY_PULL
            if self._pos.length < self.world.radius + self._size.x / 2:
                f_gravity += self._pos.normalized() * 5 * self._size.x
            f_gravity *= self._mass

        return f_gravity

    def update(self, dtime):
        turn_angle = -(self.input_axes.horizontal * copysign(1,
                       self.input_axes.vertical) * TURN_DEGREE)

        if abs(turn_angle) > 0.01:
            turn_radius = self._size.z / sin(turn_angle)
            omega = self._vel.length / turn_radius
            self._pitch += omega * dtime

        super().update(dtime)

    def to_dto(self) -> EntityDataDto:
        return EntityDataDto(
            self.eid, self._cid, self._kind,
            self._color, astuple(self._pos), self._pitch
        )
