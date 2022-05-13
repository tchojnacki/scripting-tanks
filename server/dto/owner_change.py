from attr import frozen
from dto.tagged_dto import TaggedDto


@frozen
class OwnerChangeDto(TaggedDto):
    cid: str

    @staticmethod
    def tag() -> str:
        return "owner-change"
