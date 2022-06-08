from typing import ClassVar, Literal
from attrs import frozen


@frozen
class CAddBotMsg:
    tag: ClassVar[Literal["c-add-bot"]] = "c-add-bot"
    data: None
