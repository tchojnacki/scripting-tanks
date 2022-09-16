using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

internal sealed record BroadcastOwnerChangeRequest(LID LID, CID NewOwnerCID) : IRequest;
