using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Utils.Converters;

internal sealed class AbstractJsonConverter<T> : JsonConverter<T>
{
    public override T Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        => throw new NotImplementedException($"{nameof(AbstractJsonConverter<T>)} doesn't support deserialization!");

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        foreach (var property in value.GetType().GetProperties().Where(p => p.CanRead))
        {
            writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(property.Name) ?? property.Name);
            JsonSerializer.Serialize(writer, property.GetValue(value), options);
        }
        writer.WriteEndObject();
    }
}
