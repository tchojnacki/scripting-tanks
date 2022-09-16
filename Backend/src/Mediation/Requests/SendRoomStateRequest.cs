using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

internal sealed record SendRoomStateRequest(CID CID, LID? LID) : IRequest;
