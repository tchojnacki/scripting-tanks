from __future__ import annotations
from abc import ABC, abstractmethod
from typing import Tuple, TYPE_CHECKING
from dto import InputAxesDto

if TYPE_CHECKING:
    from rooms.game_states import PlayingGameState


class AbstractAI(ABC):
    def __init__(self, world: PlayingGameState) -> None:
        self._world = world

    @abstractmethod
    def apply_inputs(self) -> Tuple[InputAxesDto, bool]:
        pass
