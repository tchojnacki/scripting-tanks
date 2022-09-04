using Backend.Contracts.Data;
using Backend.Domain;

namespace Backend.Utils.Mappers;

public static class ConnectionDataMapper
{
    public static PlayerDto ToDto(this ConnectionData data) => new()
    {
        Cid = data.Cid.Value,
        Name = data.DisplayName,
        Colors = data.Colors.ToDto(),
        Bot = data.IsBot
    };
}
