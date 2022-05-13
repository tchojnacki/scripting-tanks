from attr import frozen
from utils.uid import CID


@frozen
class PlayerDataDto:
    cid: CID
    name: str
