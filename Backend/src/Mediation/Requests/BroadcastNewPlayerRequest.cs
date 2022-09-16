using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

internal sealed record BroadcastNewPlayerRequest(LID LID, CID CID) : IRequest;
