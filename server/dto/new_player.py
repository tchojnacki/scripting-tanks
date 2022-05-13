from attr import frozen
from dto.tagged_dto import TaggedDto


@frozen
class NewPlayerDto(TaggedDto):
    cid: str
    name: str

    @staticmethod
    def tag() -> str:
        return "new-player"
