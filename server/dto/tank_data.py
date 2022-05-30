from typing import Literal, Tuple
from attrs import field, frozen
from utils.tank_colors import TankColors
from utils.uid import CID, EID


@frozen
class TankDataDto:
    kind: Literal["tank"] = field(init=False, default="tank")
    eid: EID
    cid: CID
    colors: TankColors
    pos: Tuple[float, float, float]
    pitch: float
    barrel: float
