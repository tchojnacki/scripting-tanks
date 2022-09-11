using Backend.Contracts.Data;
using Backend.Domain;

namespace Backend.Utils.Mappings;

public static class ConnectionDataMapper
{
    public static PlayerDto ToDto(this PlayerData data) => new()
    {
        Cid = data.Cid.Value,
        Name = data.Name,
        Colors = data.Colors.ToDto(),
        Bot = data.IsBot
    };
}
