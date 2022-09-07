using Backend.Contracts.Data;
using Backend.Domain;

namespace Backend.Utils.Mappings;

public static class InputAxesMapper
{
    public static InputAxes ToDomain(this InputAxesDto dto) => new()
    {
        Horizontal = dto.Horizontal,
        Vertical = dto.Vertical
    };
}