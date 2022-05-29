from .bullet_data import BulletDataDto
from .tank_data import TankDataDto

EntityDataDto = TankDataDto | BulletDataDto
