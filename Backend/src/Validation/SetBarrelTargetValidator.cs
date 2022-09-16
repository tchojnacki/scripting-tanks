using FluentValidation;
using Backend.Services;
using Backend.Domain;
using Backend.Contracts.Messages.Client;

namespace Backend.Validation;

internal sealed class SetBarrelTargetValidator : AbstractValidator<MessageContext<SetBarrelTargetClientMessage>>
{
    public SetBarrelTargetValidator(IRoomManager roomManager)
    {
        RuleFor(x => x.Sender).MustHaveAliveTank(roomManager);
    }
}
