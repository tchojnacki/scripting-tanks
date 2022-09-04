using System.Text.Json;
using System.Text.Json.Serialization;
using Backend.Contracts.Data;

namespace Backend.Utils.Converters;

public class VectorDtoJsonConverter : JsonConverter<VectorDto>
{
    public override VectorDto Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();

        reader.Read();
        var x = reader.GetDouble()!;

        reader.Read();
        var y = reader.GetDouble()!;

        reader.Read();
        var z = reader.GetDouble()!;

        reader.Read();
        if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();

        return new()
        {
            X = x,
            Y = y,
            Z = z
        };
    }

    public override void Write(Utf8JsonWriter writer, VectorDto value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(value.X);
        writer.WriteNumberValue(value.Y);
        writer.WriteNumberValue(value.Z);
        writer.WriteEndArray();
    }
}
