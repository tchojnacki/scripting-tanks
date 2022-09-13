using MediatR;
using Backend.Domain.Identifiers;

namespace Backend.Mediation.Requests;

public class AddBotRequest : IRequest
{
    public LID LID { get; }

    public AddBotRequest(LID lid) => LID = lid;
}
