from dto import FullGamePlayingStateDto
from .game_state import GameState


class PlayingGameState(GameState):
    def get_full_room_state(self) -> FullGamePlayingStateDto:
        return FullGamePlayingStateDto()
