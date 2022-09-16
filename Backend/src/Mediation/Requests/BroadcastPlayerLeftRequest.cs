using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

internal sealed record BroadcastPlayerLeftRequest(LID LID, CID CID) : IRequest;
