using FluentValidation;
using Backend.Services;
using Backend.Domain;
using Backend.Contracts.Messages.Client;

namespace Backend.Validation;

internal sealed class ShootValidator : AbstractValidator<MessageContext<ShootClientMessage>>
{
    public ShootValidator(IRoomManager roomManager)
    {
        RuleFor(x => x.Sender).MustHaveAliveTank(roomManager);
    }
}
