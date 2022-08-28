using Backend.Contracts.Data;
using Backend.Domain;
using Backend.Domain.Identifiers;

namespace Backend.Utils.Mappers;

public static class ConnectionDataMapper
{
    public static PlayerDto ToDto(this ConnectionData model, CID cid) => new()
    {
        Cid = cid.Value,
        Name = model.DisplayName,
        Colors = model.Colors.ToDto(),
        Bot = false
    };
}
