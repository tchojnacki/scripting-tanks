using Backend.Contracts.Data;
using Backend.Domain.Game;

namespace Backend.Utils.Mappings;

public static class EntityMapper
{
    public static AbstractEntityDto ToDto(this Entity entity) => entity switch
    {
        Tank tank => tank.ToDto(),
        Bullet bullet => bullet.ToDto(),
        _ => throw new NotImplementedException(),
    };

    public static TankDto ToDto(this Tank tank) => new()
    {
        Eid = tank.Eid.Value,
        Pos = tank.Pos.ToDto(),
        Cid = tank.PlayerData.Cid.Value,
        Name = tank.PlayerData.Name,
        Colors = tank.PlayerData.Colors.ToDto(),
        Pitch = tank.Pitch,
        Barrel = tank.BarrelPitch,
    };

    public static BulletDto ToDto(this Bullet bullet) => new()
    {
        Eid = bullet.Eid.Value,
        Pos = bullet.Pos.ToDto(),
        Owner = bullet.Owner.Value,
        Radius = bullet.Radius,
    };
}
