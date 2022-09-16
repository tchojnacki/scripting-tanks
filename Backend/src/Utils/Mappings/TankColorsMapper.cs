using Backend.Contracts.Data;
using Backend.Domain;

namespace Backend.Utils.Mappings;

internal static class TankColorsMapper
{
    public static TankColorsDto ToDto(this TankColors colors) => new()
    {
        TankColor = colors.TankColor,
        TurretColor = colors.TurretColor
    };

    public static TankColors ToDomain(this TankColorsDto dto) => new()
    {
        TankColor = dto.TankColor,
        TurretColor = dto.TurretColor
    };
}
