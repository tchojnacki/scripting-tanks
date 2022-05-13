from typing import ClassVar, Literal
from attr import frozen


@frozen
class CCreateLobbyMsg:
    tag: ClassVar[Literal["c-create-lobby"]] = "c-create-lobby"
    data: None
