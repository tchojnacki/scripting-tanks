from typing import Union
from dto.full_game_state import FullGameStateDto
from dto.full_menu_state import FullMenuStateDto


FullRoomStateDto = Union[FullGameStateDto, FullMenuStateDto]
