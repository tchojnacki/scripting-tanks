from math import atan2
from typing import Tuple
from dto import InputAxesDto
from models.vector import Vector
from .abstract_ai import AbstractAI


def _clamp(n):
    return max(-1, min(n, 1))


class SimpleAI(AbstractAI):
    def apply_inputs(self) -> Tuple[InputAxesDto, float, bool]:
        tanks = self._world.tanks
        my_tank = next(t for t in tanks if t.eid == self._eid)
        target = min((t for t in tanks if t.eid != self._eid),
                     key=lambda t: (my_tank.pos - t.pos).length)
        offset = target.pos - my_tank.pos
        facing = Vector.from_pitch(my_tank.pitch)

        input_axes = InputAxesDto(
            _clamp(Vector.dot(offset, facing)),
            _clamp(Vector.cross(offset, facing))
        )
        pitch = atan2(offset.x, offset.z)
        should_shoot = offset.length < 512

        return (input_axes, pitch, should_shoot)
