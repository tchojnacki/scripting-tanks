from attr import frozen
from dto.tagged_dto import TaggedDto


@frozen
class LobbyRemovedDto(TaggedDto):
    lid: str

    @staticmethod
    def tag() -> str:
        return "lobby-removed"
