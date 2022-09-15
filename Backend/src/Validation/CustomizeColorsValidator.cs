using FluentValidation;
using Backend.Services;
using Backend.Domain;
using Backend.Contracts.Messages.Client;

namespace Backend.Validation;

public class CustomizeColorsValidator : AbstractValidator<MessageContext<CustomizeColorsClientMessage>>
{
    public CustomizeColorsValidator(IRoomManager roomManager)
    {
        RuleFor(x => x.Sender).MustBeInMenu(roomManager);
        RuleFor(x => x.Message.Data).SetValidator(new TankColorsDtoValidator());
    }
}
