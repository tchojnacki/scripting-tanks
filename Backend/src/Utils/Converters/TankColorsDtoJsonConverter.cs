using System.Text.Json;
using System.Text.Json.Serialization;
using Backend.Contracts.Data;

namespace Backend.Utils.Converters;

public class TankColorsDtoJsonConverter : JsonConverter<TankColorsDto>
{
    public override TankColorsDto Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();

        reader.Read();
        var tankColor = reader.GetString()!;

        reader.Read();
        var turretColor = reader.GetString()!;

        reader.Read();
        if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();

        return new()
        {
            TankColor = tankColor,
            TurretColor = turretColor
        };
    }

    public override void Write(Utf8JsonWriter writer, TankColorsDto value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteStringValue(value.TankColor);
        writer.WriteStringValue(value.TurretColor);
        writer.WriteEndArray();
    }
}
