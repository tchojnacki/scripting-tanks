from typing import Union
from .full_game_state import FullGameStateDto
from .full_menu_state import FullMenuStateDto


FullRoomStateDto = Union[FullGameStateDto, FullMenuStateDto]
