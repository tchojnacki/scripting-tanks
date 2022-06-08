from typing import Tuple
from dto import InputAxesDto
from .abstract_ai import AbstractAI


class SimpleAI(AbstractAI):
    def apply_inputs(self) -> Tuple[InputAxesDto, bool]:
        return (InputAxesDto(1, 0), True)
