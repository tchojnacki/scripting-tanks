using Backend.Contracts.Data;
using Backend.Domain.Game;

namespace Backend.Utils.Mappings;

public static class EntityMapper
{
    public static AbstractEntityDto ToDto(this Entity entity) => entity switch
    {
        Tank t => t.ToDto(),
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
        Barrel = tank.BarrelTarget
    };
}
