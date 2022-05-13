from abc import ABC, abstractmethod
from attr import frozen


@frozen
class TaggedDto(ABC):
    @staticmethod
    @abstractmethod
    def tag() -> str:
        pass
