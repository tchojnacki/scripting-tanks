using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public record BroadcastRoomStateRequest(LID LID) : IRequest;
