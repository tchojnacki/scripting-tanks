from __future__ import annotations
import random
from typing import TYPE_CHECKING
from dto import FullGameStateDto, PlayerDataDto, LobbyDataDto, ScoreboardEntryDto
from messages.client import ClientMsg
from messages.server import SOwnerChangeMsg, SFullRoomStateMsg
from rooms.game_states import WaitingGameState, PlayingGameState, GameState, SummaryGameState
from utils.uid import CID, LID
from .connection_room import ConnectionRoom

if TYPE_CHECKING:
    from .room_manager import RoomManager


class GameRoom(ConnectionRoom):
    def __init__(self, room_manager: RoomManager, owner: CID, lid: LID, name: str):
        super().__init__(room_manager)
        self.owner = owner
        self.lid = lid
        self.name = name
        self._state: GameState = WaitingGameState(self)

    def handle_message(self, sender_cid: CID, cmsg: ClientMsg):
        self._state.handle_message(sender_cid, cmsg)

    async def _switch_state(self, state_cls: type[GameState], **kwargs):
        self._state = state_cls(self, **kwargs)
        await self.broadcast_message(SFullRoomStateMsg(self.get_full_room_state()))
        await self._room_manager.upsert_lobby(self)

    async def start_game(self):
        if isinstance(self._state, WaitingGameState):
            await self._switch_state(PlayingGameState)

    async def show_summary(self, scoreboard: list[ScoreboardEntryDto]):
        if isinstance(self._state, PlayingGameState):
            await self._switch_state(SummaryGameState, scoreboard=scoreboard)

    async def play_again(self):
        if isinstance(self._state, SummaryGameState):
            await self._switch_state(WaitingGameState)

    async def close_lobby(self):
        await self._room_manager.close_lobby(self)

    async def promote(self, target: CID):
        if self.has_player(target):
            self.owner = target
            await self.broadcast_message(SOwnerChangeMsg(self.owner))

    async def kick(self, target: CID):
        if self.has_player(target):
            await self._room_manager.kick(target)

    async def add_bot(self):
        await self._room_manager.add_bot(self)

    def get_full_room_state(self) -> FullGameStateDto:
        return self._state.get_full_room_state()

    async def on_join(self, joiner_cid: CID):
        await self._state.on_join(joiner_cid)
        await super().on_join(joiner_cid)

    async def on_leave(self, leaver_cid: CID):
        await super().on_leave(leaver_cid)
        await self._state.on_leave(leaver_cid)
        if leaver_cid == self.owner and len(self._player_ids) > 0:
            self.owner = random.choice(tuple(self._player_ids))
            await self.broadcast_message(SOwnerChangeMsg(self.owner))

    @property
    def players(self) -> list[PlayerDataDto]:
        return [
            self.cid_to_player_data(cid)
            for cid in self._player_ids
        ]

    @property
    def lobby_data(self) -> LobbyDataDto:
        return LobbyDataDto(
            self.lid,
            self.name,
            len(self.players),
            self.get_full_room_state().location
        )
