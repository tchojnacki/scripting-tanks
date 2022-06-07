from attrs import frozen
from utils.uid import CID


@frozen
class ScoreboardEntryDto:
    cid: CID
    name: str
    score: int
