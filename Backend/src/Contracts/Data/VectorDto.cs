using System.Text.Json.Serialization;
using Backend.Utils.Converters;

namespace Backend.Contracts.Data;

[JsonConverter(typeof(VectorDtoJsonConverter))]
public record VectorDto
{
    public double X { get; init; }
    public double Y { get; init; }
    public double Z { get; init; }
}
