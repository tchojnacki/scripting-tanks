using Backend.Domain.Identifiers;
using MediatR;

namespace Backend.Mediation.Requests;

internal sealed record SendRoomStateRequest(Cid Cid, Lid? Lid) : IRequest;
