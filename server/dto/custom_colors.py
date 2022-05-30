import re
from attrs import frozen, field
from utils.tank_colors import TankColors

_hex_pattern = re.compile("^#[a-fA-F0-9]{6}$")


def _colors_validator(_i, _a, value):
    if len(value) != 2:
        raise ValueError("Tank must have two colors!")
    for color in value:
        if not _hex_pattern.match(color):
            raise ValueError("Color is invalid!")


@frozen
class CustomColorsDto:
    colors: TankColors = field(validator=_colors_validator)
