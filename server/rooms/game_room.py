from connection_data import ConnectionData
from .connection_room import ConnectionRoom


class GameRoom(ConnectionRoom):
    def get_full_room_state(self):
        return {
            "location": "lobby",
            "players": list(self._player_ids),
        }

    async def on_join(self, joiner: "ConnectionData"):
        await self.broadcast_message("new-player", joiner.cid)
        await super().on_join(joiner)

    async def on_leave(self, leaver: "ConnectionData"):
        await super().on_leave(leaver)
        await self.broadcast_message("player-left", leaver.cid)

    def handle_message(self, sender: ConnectionData, tag: str, data: any):
        if tag == "leave-lobby":
            self.manager.return_to_menu(sender.cid)
