from __future__ import annotations
from abc import ABC, abstractmethod
from typing import Tuple, TYPE_CHECKING
from dto import InputAxesDto
from utils.uid import EID

if TYPE_CHECKING:
    from rooms.game_states import PlayingGameState


class AbstractAI(ABC):
    def __init__(self, eid: EID, world: PlayingGameState) -> None:
        self._eid = eid
        self._world = world

    @abstractmethod
    def apply_inputs(self) -> Tuple[InputAxesDto, float, bool]:
        pass
