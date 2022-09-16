using Backend.Contracts.Data;
using Backend.Domain.Game;

namespace Backend.Utils.Mappings;

internal static class VectorMapper
{
    public static VectorDto ToDto(this Vector vector) => new()
    {
        X = vector.X,
        Y = vector.Y,
        Z = vector.Z
    };
}
