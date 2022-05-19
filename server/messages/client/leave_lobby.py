from typing import ClassVar, Literal
from attrs import frozen


@frozen
class CLeaveLobbyMsg:
    tag: ClassVar[Literal["c-leave-lobby"]] = "c-leave-lobby"
    data: None
