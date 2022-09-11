using Backend.Contracts.Data;

namespace Backend.Contracts.Messages.Server;

public record RoomStateServerMessage : IServerMessage<AbstractRoomStateDto>
{
    public string Tag => "s-full-room-state";
    public AbstractRoomStateDto Data { get; init; } = default!;
}
