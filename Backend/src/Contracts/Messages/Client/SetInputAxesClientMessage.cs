using Backend.Contracts.Data;

namespace Backend.Contracts.Messages.Client;

public record SetInputAxesClientMessage : IClientMessage<InputAxesDto>
{
    public string Tag => "c-set-input-axes";
    public InputAxesDto Data { get; init; } = default!;
}
