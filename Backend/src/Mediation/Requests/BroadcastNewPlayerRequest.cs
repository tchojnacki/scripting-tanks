using Backend.Domain.Identifiers;
using MediatR;

namespace Backend.Mediation.Requests;

internal sealed record BroadcastNewPlayerRequest(Lid Lid, Cid Cid) : IRequest;