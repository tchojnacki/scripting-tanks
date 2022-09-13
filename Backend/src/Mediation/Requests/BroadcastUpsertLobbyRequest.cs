using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public class BroadcastUpsertLobbyRequest : IRequest
{
    public LID LID { get; }

    public BroadcastUpsertLobbyRequest(LID lid) => LID = lid;
}
