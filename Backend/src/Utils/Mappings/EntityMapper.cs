using Backend.Contracts.Data;
using Backend.Domain.Game;

namespace Backend.Utils.Mappings;

internal static class EntityMapper
{
    public static AbstractEntityDto ToDto(this Entity entity) => entity switch
    {
        Tank tank => tank.ToDto(),
        Bullet bullet => bullet.ToDto(),
        _ => throw new NotImplementedException()
    };

    public static TankDto ToDto(this Tank tank) => new()
    {
        Eid = tank.Eid.ToString(),
        Pos = tank.Position.ToDto(),
        Cid = tank.PlayerData.Cid.ToString(),
        Name = tank.PlayerData.Name,
        Colors = tank.PlayerData.Colors.ToDto(),
        Pitch = tank.Pitch,
        Barrel = tank.BarrelPitch
    };

    private static BulletDto ToDto(this Bullet bullet) => new()
    {
        Eid = bullet.Eid.ToString(),
        Pos = bullet.Position.ToDto(),
        Owner = bullet.OwnerCid.ToString(),
        Radius = bullet.Radius
    };
}
