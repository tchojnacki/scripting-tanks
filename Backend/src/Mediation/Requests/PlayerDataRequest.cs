using Backend.Domain;
using Backend.Domain.Identifiers;
using MediatR;

namespace Backend.Mediation.Requests;

internal sealed record PlayerDataRequest(Cid Cid) : IRequest<PlayerData>;
