using FluentValidation;
using Backend.Services;
using Backend.Domain;
using Backend.Domain.Rooms.States;
using Backend.Contracts.Messages.Client;

namespace Backend.Validation;

public class CloseLobbyValidator : AbstractValidator<MessageContext<CloseLobbyClientMessage>>
{
    public CloseLobbyValidator(IRoomManager roomManager)
    {
        RuleFor(x => x.Sender).MustBeRoomOwner(gr => gr is WaitingGameState, roomManager);
    }
}
