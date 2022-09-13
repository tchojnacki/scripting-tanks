using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public record BroadcastUpsertLobbyRequest(LID LID) : IRequest;
