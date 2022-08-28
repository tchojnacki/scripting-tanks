using Backend.Contracts.Data;
using Backend.Domain;

namespace Backend.Utils.Mappers;

public static class TankColorsMapper
{
    public static TankColorsDto ToDto(this TankColors model) => new()
    {
        TankColor = model.TankColor,
        TurretColor = model.TurretColor
    };

    public static TankColors ToDomain(this TankColorsDto dto) => new()
    {
        TankColor = dto.TankColor,
        TurretColor = dto.TurretColor
    };
}
