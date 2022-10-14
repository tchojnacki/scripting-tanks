using Backend.Domain.Identifiers;
using MediatR;

namespace Backend.Mediation.Requests;

internal sealed record PlayAgainRequest(Lid Lid) : IRequest;
