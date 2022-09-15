using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public record BroadcastLobbyRemovedRequest(LID LID) : IRequest;
