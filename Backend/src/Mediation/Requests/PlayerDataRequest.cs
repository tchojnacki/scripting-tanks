using MediatR;
using Backend.Domain;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

internal sealed record PlayerDataRequest(CID CID) : IRequest<PlayerData>;
