from typing import ClassVar, Literal
from attrs import frozen


@frozen
class CStartGameMsg:
    tag: ClassVar[Literal["c-start-game"]] = "c-start-game"
    data: None
