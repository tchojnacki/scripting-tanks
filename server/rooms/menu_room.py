import asyncio
from connection_data import ConnectionData
from .connection_room import ConnectionRoom


class MenuRoom(ConnectionRoom):
    def get_full_room_state(self):
        return {
            "location": "menu",
            "lobbies": [{"name": i} for i, _ in enumerate(self.manager.lobby_list)],
        }

    def handle_message(self, sender: ConnectionData, tag: str, data: any):
        if tag == "create-lobby":
            i = self.manager.create_lobby()
            asyncio.ensure_future(sender.room.broadcast_message("new-lobby", {"name": i}))
        elif tag == "enter-lobby":
            self.manager.join_lobby(sender.cid, data)
