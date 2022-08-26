using Backend.Contracts.Data;

namespace Backend.Contracts.Messages.Client;

public record CustomizeColorsClientMessage : IClientMessage<CustomColorsDto>
{
    public string Tag => "c-customize-colors";
    public CustomColorsDto Data { get; init; } = default!;
}
