using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public class BroadcastLobbyRemovedRequest : IRequest
{
    public LID LID { get; }

    public BroadcastLobbyRemovedRequest(LID lid) => LID = lid;
}
