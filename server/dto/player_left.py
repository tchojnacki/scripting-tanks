from attr import frozen
from dto.tagged_dto import TaggedDto


@frozen
class PlayerLeftDto(TaggedDto):
    cid: str

    @staticmethod
    def tag() -> str:
        return "player-left"
