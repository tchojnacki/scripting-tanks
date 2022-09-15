using MediatR;
using Backend.Domain;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public record PlayerDataRequest(CID CID) : IRequest<PlayerData>;
