from attr import frozen
from dto.tagged_dto import TaggedDto


@frozen
class NewLobbyDto(TaggedDto):
    lid: str
    name: str
    players: int

    @staticmethod
    def tag() -> str:
        return "new-lobby"
