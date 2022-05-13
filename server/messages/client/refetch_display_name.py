from typing import ClassVar, Literal
from attr import frozen


@frozen
class CRefetchDisplayNameMsg:
    tag: ClassVar[Literal["c-refetch-display-name"]] = "c-refetch-display-name"
    data: None
