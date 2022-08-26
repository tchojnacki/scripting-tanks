namespace Backend.Contracts.Messages.Client;

public record SetBarrelTargetClientMessage : IClientMessage<double>
{
    public string Tag => "c-set-barrel-target";
    public double Data { get; init; } = default!;
}
