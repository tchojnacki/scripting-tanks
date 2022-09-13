using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public record KickRequest(CID CID) : IRequest;
