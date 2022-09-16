using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

internal sealed record PlayAgainRequest(LID LID) : IRequest;
