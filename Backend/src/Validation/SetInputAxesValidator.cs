using FluentValidation;
using Backend.Services;
using Backend.Domain;
using Backend.Contracts.Messages.Client;

namespace Backend.Validation;

internal sealed class SetInputAxesValidator : AbstractValidator<MessageContext<SetInputAxesClientMessage>>
{
    public SetInputAxesValidator(IRoomManager roomManager)
    {
        RuleFor(x => x.Sender).MustHaveAliveTank(roomManager);
        RuleFor(x => x.Message.Data).SetValidator(new InputAxesDtoValidator());
    }
}
