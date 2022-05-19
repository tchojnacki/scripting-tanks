from typing import ClassVar, Literal
from attrs import frozen


@frozen
class CShootMsg:
    tag: ClassVar[Literal["c-shoot"]] = "c-shoot"
    data: None
