from typing import ClassVar, Literal
from attrs import frozen


@frozen
class CCloseLobbyMsg:
    tag: ClassVar[Literal["c-close-lobby"]] = "c-close-lobby"
    data: None
