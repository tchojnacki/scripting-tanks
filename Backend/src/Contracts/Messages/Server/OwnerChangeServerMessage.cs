namespace Backend.Contracts.Messages.Server;

public record OwnerChangeServerMessage : IServerMessage<string>
{
    public string Tag => "s-owner-change";
    public string Data { get; init; } = default!;
}
