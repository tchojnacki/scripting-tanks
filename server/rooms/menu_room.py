from dto import FullMenuStateDto
from .connection_room import ConnectionRoom


class MenuRoom(ConnectionRoom):
    def get_full_room_state(self) -> FullMenuStateDto:
        return FullMenuStateDto(self._room_manager.lobby_entries)
