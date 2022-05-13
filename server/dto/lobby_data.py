from attr import frozen


@frozen
class LobbyDataDto:
    lid: str
    name: str
    players: int
