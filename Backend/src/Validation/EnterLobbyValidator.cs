using FluentValidation;
using Backend.Services;
using Backend.Domain;
using Backend.Domain.Identifiers;
using Backend.Contracts.Messages.Client;

namespace Backend.Validation;

internal sealed class EnterLobbyValidator : AbstractValidator<MessageContext<EnterLobbyClientMessage>>
{
    public EnterLobbyValidator(IRoomManager roomManager)
    {
        RuleFor(x => x.Sender).MustBeInMenu(roomManager);
        Transform(from: x => x.Message.Data, to: LID.Deserialize).MustBeAValidGameRoom(roomManager);
    }
}
