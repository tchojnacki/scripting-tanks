from attrs import field, frozen


def _axis_validator(_i, _a, value):
    if not -1 <= value <= 1:
        raise ValueError("Value must be between -1 and 1!")


@frozen
class InputAxesDto:
    vertical: float = field(validator=_axis_validator)
    horizontal: float = field(validator=_axis_validator)
