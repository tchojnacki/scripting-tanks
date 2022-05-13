from attr import frozen
from dto.tagged_dto import TaggedDto


@frozen
class AssignDisplayNameDto(TaggedDto):
    name: str

    @staticmethod
    def tag() -> str:
        return "assign-display-name"
