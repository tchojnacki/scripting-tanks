from typing import ClassVar, Literal
from attrs import frozen
from utils.uid import CID


@frozen
class CKickPlayerMsg:
    tag: ClassVar[Literal["c-kick-player"]] = "c-kick-player"
    data: CID
