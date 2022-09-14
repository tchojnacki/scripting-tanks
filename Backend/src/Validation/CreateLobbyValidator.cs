using FluentValidation;
using Backend.Services;
using Backend.Domain;
using Backend.Contracts.Messages.Client;

namespace Backend.Validation;

public class CreateLobbyValidator : AbstractValidator<MessageContext<CreateLobbyClientMessage>>
{
    public CreateLobbyValidator(IRoomManager roomManager)
    {
        RuleFor(x => x.Sender).MustBeInMenu(roomManager);
    }
}
