from typing import Union
from .full_game_playing_state import FullGamePlayingStateDto
from .full_game_waiting_state import FullGameWaitingStateDto

FullGameStateDto = Union[FullGamePlayingStateDto, FullGameWaitingStateDto]
