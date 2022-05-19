from typing import ClassVar, Literal
from attrs import frozen
from utils.uid import LID


@frozen
class CEnterLobbyMsg:
    tag: ClassVar[Literal["c-enter-lobby"]] = "c-enter-lobby"
    data: LID
