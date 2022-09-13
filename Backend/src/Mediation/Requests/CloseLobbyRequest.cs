using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public record CloseLobbyRequest(LID LID) : IRequest;
