import asyncio
from dto import FullGameWaitingStateDto, PlayerDataDto
from messages.client import ClientMsg, CStartGameMsg
from messages.server import SNewPlayerMsg, SPlayerLeftMsg
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

    async def on_join(self, joiner_cid: CID):
        await self._room.broadcast_message(SNewPlayerMsg(PlayerDataDto(
            joiner_cid, self._room.cid_to_display_name(joiner_cid)
        )))

    async def on_leave(self, leaver_cid: CID):
        await self._room.broadcast_message(SPlayerLeftMsg(leaver_cid))
