using System.Text.Json.Serialization;
using Backend.Utils.Converters;

namespace Backend.Contracts.Data;

[JsonConverter(typeof(VectorDtoJsonConverter))]
public readonly record struct VectorDto
{
    public readonly double X { get; init; }
    public readonly double Y { get; init; }
    public readonly double Z { get; init; }
}
