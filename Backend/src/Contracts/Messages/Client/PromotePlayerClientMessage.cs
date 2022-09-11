namespace Backend.Contracts.Messages.Client;

public record PromotePlayerClientMessage : IClientMessage<string>
{
    public string Tag => "c-promote-player";
    public string Data { get; init; } = default!;
}
