from attr import frozen
from dto.tagged_dto import TaggedDto


@frozen
class FullRoomStateDto(TaggedDto):
    @staticmethod
    def tag() -> str:
        return "full-room-state"
