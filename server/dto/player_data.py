from attr import frozen


@frozen
class PlayerDataDto:
    cid: str
    name: str
