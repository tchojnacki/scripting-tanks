from typing import ClassVar, Literal
from attrs import frozen


@frozen
class CRerollNameMsg:
    tag: ClassVar[Literal["c-reroll-name"]] = "c-reroll-name"
    data: None
