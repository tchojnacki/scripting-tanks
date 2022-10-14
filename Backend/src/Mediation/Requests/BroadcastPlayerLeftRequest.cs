using Backend.Domain.Identifiers;
using MediatR;

namespace Backend.Mediation.Requests;

internal sealed record BroadcastPlayerLeftRequest(Lid Lid, Cid Cid) : IRequest;
