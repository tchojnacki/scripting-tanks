from typing import ClassVar, Literal
from attrs import frozen
from dto import InputAxesDto


@frozen
class CSetInputAxesMsg:
    tag: ClassVar[Literal["c-set-input-axes"]] = "c-set-input-axes"
    data: InputAxesDto
