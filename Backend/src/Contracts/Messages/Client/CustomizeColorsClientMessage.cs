using Backend.Contracts.Data;

namespace Backend.Contracts.Messages.Client;

public record CustomizeColorsClientMessage : IClientMessage<ColorCustomizationDto>
{
    public string Tag => "c-customize-colors";
    public ColorCustomizationDto Data { get; init; } = default!;
}
