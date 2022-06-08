from attrs import field, frozen
from utils.tank_colors import TankColors
from utils.uid import CID


@frozen
class PlayerDataDto:
    cid: CID
    name: str
    colors: TankColors
    bot: bool = field(default=False)
