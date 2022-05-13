from attr import frozen
from utils.uid import LID


@frozen
class LobbyDataDto:
    lid: LID
    name: str
    players: int
