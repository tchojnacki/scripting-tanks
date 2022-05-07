from .connection_room import ConnectionRoom


class MenuRoom(ConnectionRoom):
    def get_full_room_state(self) -> any:
        return {
            "location": "menu",
            "lobbies": self._room_manager.lobby_entries,
        }
