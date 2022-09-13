using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public record BroadcastPlayerLeftRequest(LID LID, CID CID) : IRequest;
