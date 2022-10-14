using Backend.Domain.Identifiers;
using MediatR;

namespace Backend.Mediation.Requests;

internal sealed record BroadcastOwnerChangeRequest(Lid Lid, Cid NewOwnerCid) : IRequest;
