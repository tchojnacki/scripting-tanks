using Backend.Contracts.Data;
using Backend.Domain.Game;

namespace Backend.Utils.Mappings;

internal static class EntityMapper
{
    public static AbstractEntityDto ToDto(this Entity entity) => entity switch
    {
        Tank tank => tank.ToDto(),
        Bullet bullet => bullet.ToDto(),
        _ => throw new NotImplementedException(),
    };

    public static TankDto ToDto(this Tank tank) => new()
    {
        EID = tank.EID.ToString(),
        Pos = tank.Position.ToDto(),
        CID = tank.PlayerData.CID.ToString(),
        Name = tank.PlayerData.Name,
        Colors = tank.PlayerData.Colors.ToDto(),
        Pitch = tank.Pitch,
        Barrel = tank.BarrelPitch,
    };

    public static BulletDto ToDto(this Bullet bullet) => new()
    {
        EID = bullet.EID.ToString(),
        Pos = bullet.Position.ToDto(),
        Owner = bullet.OwnerCID.ToString(),
        Radius = bullet.Radius,
    };
}
