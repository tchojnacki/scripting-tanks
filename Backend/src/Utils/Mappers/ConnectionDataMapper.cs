using Backend.Contracts.Data;
using Backend.Domain;

namespace Backend.Utils.Mappers;

public static class ConnectionDataMapper
{
    public static PlayerDto ToDto(this ConnectionData model) => new()
    {
        Cid = model.Cid.Value,
        Name = model.DisplayName,
        Colors = model.Colors.ToDto(),
        Bot = false
    };
}
