from typing import ClassVar, Literal
from attr import frozen


@frozen
class CStartGameMsg:
    tag: ClassVar[Literal["c-start-game"]] = "c-start-game"
    data: None
