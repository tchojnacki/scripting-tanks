import asyncio
from dto import FullGameWaitingStateDto
from messages.client import ClientMsg, CAddBotMsg, CStartGameMsg, \
    CCloseLobbyMsg, CPromotePlayerMsg, CKickPlayerMsg
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

    def handle_message(self, sender_cid: CID, cmsg: ClientMsg):
        is_owner = sender_cid == self._room.owner
        match cmsg:
            case CStartGameMsg() if is_owner and len(self._room.players) >= 2:
                asyncio.ensure_future(self._room.start_game())
            case CCloseLobbyMsg() if is_owner:
                asyncio.ensure_future(self._room.close_lobby())
            case CPromotePlayerMsg(target) if is_owner:
                asyncio.ensure_future(self._room.promote(target))
            case CKickPlayerMsg(target) if is_owner:
                asyncio.ensure_future(self._room.kick(target))
            case CAddBotMsg() if is_owner:
                asyncio.ensure_future(self._room.add_bot())

    async def on_join(self, joiner_cid: CID):
        await self._room.broadcast_message(SNewPlayerMsg(
            self._room.cid_to_player_data(joiner_cid)
        ))

    async def on_leave(self, leaver_cid: CID):
        await self._room.broadcast_message(SPlayerLeftMsg(leaver_cid))
