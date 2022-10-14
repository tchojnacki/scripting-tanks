using Backend.Domain.Identifiers;
using MediatR;

namespace Backend.Mediation.Requests;

internal sealed record BroadcastLobbyRemovedRequest(Lid Lid) : IRequest;