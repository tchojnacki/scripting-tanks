using FluentValidation;
using Backend.Services;
using Backend.Domain;
using Backend.Contracts.Messages.Client;

namespace Backend.Validation;

internal sealed class LeaveLobbyValidator : AbstractValidator<MessageContext<LeaveLobbyClientMessage>>
{
    public LeaveLobbyValidator(IRoomManager roomManager)
    {
        RuleFor(x => x.Sender).MustBeInGameRoom(roomManager);
    }
}
