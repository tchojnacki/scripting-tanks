using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

internal sealed record BroadcastRoomStateRequest(LID LID) : IRequest;
