using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

internal sealed record ShowSummaryRequest(LID LID) : IRequest;