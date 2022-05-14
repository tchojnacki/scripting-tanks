from typing import Union
from .assign_identity import SAssignIdentityMsg
from .full_room_state import SFullRoomStateMsg
from .lobby_removed import SLobbyRemovedMsg
from .new_player import SNewPlayerMsg
from .owner_change import SOwnerChangeMsg
from .player_left import SPlayerLeftMsg
from .upsert_lobby import SUpsertLobbyMsg


ServerMsg = Union[SAssignIdentityMsg, SFullRoomStateMsg,
                  SLobbyRemovedMsg, SNewPlayerMsg, SOwnerChangeMsg, SPlayerLeftMsg, SUpsertLobbyMsg]
