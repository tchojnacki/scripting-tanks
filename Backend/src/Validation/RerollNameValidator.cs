using FluentValidation;
using Backend.Services;
using Backend.Domain;
using Backend.Contracts.Messages.Client;

namespace Backend.Validation;

public class RerollNameValidator : AbstractValidator<MessageContext<RerollNameClientMessage>>
{
    public RerollNameValidator(IRoomManager roomManager)
    {
        RuleFor(x => x.CID).Must(cid => roomManager.RoomContaining(cid) == roomManager.MenuRoom);
    }
}
