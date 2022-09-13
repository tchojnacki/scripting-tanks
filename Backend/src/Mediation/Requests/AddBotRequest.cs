using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public record AddBotRequest(LID LID) : IRequest;
