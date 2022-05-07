import asyncio
from connection_data import ConnectionData
from .connection_room import ConnectionRoom


class MenuRoom(ConnectionRoom):
    def get_full_room_state(self):
        return {
            "location": "menu",
            "lobbies": [
                {"lid": room.lid, "name": room.name}
                for _, room in self.manager.lobby_entries
            ],
        }

    def handle_message(self, sender: ConnectionData, tag: str, data: any):
        if tag == "create-lobby":
            asyncio.ensure_future(self.manager.join_lobby(sender.cid, sender.cid))
        elif tag == "enter-lobby":
            asyncio.ensure_future(self.manager.join_lobby(sender.cid, data))
