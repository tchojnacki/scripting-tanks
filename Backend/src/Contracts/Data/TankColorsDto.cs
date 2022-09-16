using System.Text.Json.Serialization;
using Backend.Utils.Converters;

namespace Backend.Contracts.Data;

[JsonConverter(typeof(TankColorsDtoJsonConverter))]
public sealed record TankColorsDto
{
    public string TankColor { get; init; } = default!;
    public string TurretColor { get; init; } = default!;
}
