from attrs import frozen
from utils.tank_colors import TankColors
from utils.uid import CID


@frozen
class PlayerDataDto:
    cid: CID
    name: str
    colors: TankColors
