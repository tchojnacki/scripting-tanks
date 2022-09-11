using System.Text.Json.Serialization;
using Backend.Utils.Converters;

namespace Backend.Contracts.Data;

[JsonConverter(typeof(AbstractJsonConverter<AbstractRoomStateDto>))]
public abstract record AbstractRoomStateDto
{
    public abstract string Location { get; }
}
