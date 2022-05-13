from typing import ClassVar, Literal
from attr import frozen


@frozen
class CLeaveLobbyMsg:
    tag: ClassVar[Literal["c-leave-lobby"]] = "c-leave-lobby"
    data: None
