import asyncio
from dto import FullGameWaitingStateDto
from messages.client import ClientMsg, CStartGameMsg
from utils.uid import CID
from .game_state import GameState


class WaitingGameState(GameState):
    def get_full_room_state(self) -> FullGameWaitingStateDto:
        return FullGameWaitingStateDto(
            self._room.name,
            self._room.owner,
            self._room.players,
        )

    def is_joinable(self) -> bool:
        return True

    def handle_message(self, sender_cid: CID, cmsg: ClientMsg):
        match cmsg:
            case CStartGameMsg():
                asyncio.ensure_future(self._room.start_game())
