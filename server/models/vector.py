from __future__ import annotations
from math import sqrt
from attr import define


@define
class Vector:
    x: float
    y: float
    z: float

    @property
    def length(self) -> float:
        return sqrt(self.x ** 2 + self.y ** 2 + self.z ** 2)

    @staticmethod
    def zero() -> Vector:
        return Vector(0, 0, 0)

    @staticmethod
    def dot(a: Vector, b: Vector) -> float:
        return a.x * b.x + a.y * b.y + a.z * b.z

    def __iadd__(self, other):
        if isinstance(other, Vector):
            self.x = self.x + other.x
            self.y = self.y + other.y
            self.z = self.z + other.z
            return self

    def __add__(self, other):
        if isinstance(other, Vector):
            return Vector(self.x + other.x, self.y + other.y, self.z + other.z)

    def __mul__(self, other):
        if isinstance(other, (int, float)):
            return Vector(self.x * other, self.y * other, self.z * other)

    def __truediv__(self, other):
        return self.__mul__(1 / other)

    def __rmul__(self, other):
        return self.__mul__(other)
