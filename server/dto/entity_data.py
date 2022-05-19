from typing import Union
from .bullet_data import BulletDataDto
from .tank_data import TankDataDto


EntityDataDto = Union[TankDataDto, BulletDataDto]
