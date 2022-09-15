using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public record SendRoomStateRequest(CID CID, LID? LID) : IRequest;
