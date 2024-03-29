using Backend.Contracts.Data;
using Backend.Domain.Game;

namespace Backend.Utils.Mappings;

internal static class InputAxesMapper
{
    public static InputAxes ToDomain(this InputAxesDto dto) => new()
    {
        Horizontal = dto.Horizontal,
        Vertical = dto.Vertical
    };
}
