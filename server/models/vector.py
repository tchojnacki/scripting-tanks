from __future__ import annotations
from attr import define


@define
class Vector:
    x: float
    y: float
    z: float

    @staticmethod
    def zero() -> Vector:
        return Vector(0, 0, 0)
