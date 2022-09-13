using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public class BroadcastRoomStateRequest : IRequest
{
    public LID LID { get; }

    public BroadcastRoomStateRequest(LID lid) => LID = lid;
}
