using FluentValidation;
using Backend.Services;
using Backend.Domain;
using Backend.Contracts.Messages.Client;

namespace Backend.Validation;

internal sealed class RerollNameValidator : AbstractValidator<MessageContext<RerollNameClientMessage>>
{
    public RerollNameValidator(IRoomManager roomManager)
    {
        RuleFor(x => x.Sender).MustBeInMenu(roomManager);
    }
}
