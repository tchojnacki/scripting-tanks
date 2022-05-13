from typing import Union
from messages.assign_display_name import SAssignDisplayNameMsg
from messages.full_room_state import SFullRoomStateMsg
from messages.lobby_removed import SLobbyRemovedMsg
from messages.new_lobby import SNewLobbyMsg
from messages.new_player import SNewPlayerMsg
from messages.owner_change import SOwnerChangeMsg
from messages.player_left import SPlayerLeftMsg


ServerMsg = Union[SAssignDisplayNameMsg, SFullRoomStateMsg, SLobbyRemovedMsg,
                  SNewLobbyMsg, SNewPlayerMsg, SOwnerChangeMsg, SPlayerLeftMsg]
