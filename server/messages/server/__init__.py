from typing import Union
from .assign_display_name import SAssignDisplayNameMsg
from .full_room_state import SFullRoomStateMsg
from .lobby_removed import SLobbyRemovedMsg
from .new_lobby import SNewLobbyMsg
from .new_player import SNewPlayerMsg
from .owner_change import SOwnerChangeMsg
from .player_left import SPlayerLeftMsg


ServerMsg = Union[SAssignDisplayNameMsg, SFullRoomStateMsg, SLobbyRemovedMsg,
                  SNewLobbyMsg, SNewPlayerMsg, SOwnerChangeMsg, SPlayerLeftMsg]
