from typing import ClassVar, Literal
from attrs import frozen
from utils.uid import CID


@frozen
class CPromotePlayerMsg:
    tag: ClassVar[Literal["c-promote-player"]] = "c-promote-player"
    data: CID
