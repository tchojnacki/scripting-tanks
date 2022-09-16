using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public record PlayAgainRequest(LID LID) : IRequest;
