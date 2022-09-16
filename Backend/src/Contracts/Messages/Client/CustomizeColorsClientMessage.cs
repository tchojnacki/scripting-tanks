using Backend.Contracts.Data;

namespace Backend.Contracts.Messages.Client;

public sealed record CustomizeColorsClientMessage : IClientMessage<TankColorsDto>
{
    public string Tag => "c-customize-colors";
    public TankColorsDto Data { get; init; } = default!;
}
