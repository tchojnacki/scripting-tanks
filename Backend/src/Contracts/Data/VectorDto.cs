using System.Text.Json.Serialization;
using Backend.Utils.Converters;

namespace Backend.Contracts.Data;

[JsonConverter(typeof(VectorDtoJsonConverter))]
public record VectorDto
{
    public double X { get; init; } = default!;
    public double Y { get; init; } = default!;
    public double Z { get; init; } = default!;
}
