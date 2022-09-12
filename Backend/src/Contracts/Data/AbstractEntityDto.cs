using System.Text.Json.Serialization;
using Backend.Utils.Converters;

namespace Backend.Contracts.Data;

[JsonConverter(typeof(AbstractJsonConverter<AbstractEntityDto>))]
public abstract record AbstractEntityDto
{
    public abstract string Kind { get; }
    public string EID { get; init; } = default!;
    public VectorDto Pos { get; init; } = default!;
}
