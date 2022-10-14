using Backend.Domain.Identifiers;
using MediatR;

namespace Backend.Mediation.Requests;

internal sealed record BroadcastUpsertLobbyRequest(Lid Lid) : IRequest;
