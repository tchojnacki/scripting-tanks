using Backend.Contracts.Data;
using Backend.Domain;

namespace Backend.Utils.Mappings;

internal static class PlayerDataMapper
{
    public static PlayerDto ToDto(this PlayerData data) => new()
    {
        Cid = data.Cid.ToString(),
        Name = data.Name,
        Colors = data.Colors.ToDto(),
        Bot = data.IsBot
    };
}