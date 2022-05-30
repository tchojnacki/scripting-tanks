from typing import ClassVar, Literal
from attrs import frozen
from dto import CustomColorsDto


@frozen
class CCustomizeColorsMsg:
    tag: ClassVar[Literal["c-customize-colors"]] = "c-customize-colors"
    data: CustomColorsDto
