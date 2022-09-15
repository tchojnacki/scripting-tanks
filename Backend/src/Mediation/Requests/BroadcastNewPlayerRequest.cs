using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public record BroadcastNewPlayerRequest(LID LID, CID CID) : IRequest;
