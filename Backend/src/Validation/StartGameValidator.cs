using FluentValidation;
using Backend.Services;
using Backend.Domain;
using Backend.Domain.Rooms.GameStates;
using Backend.Contracts.Messages.Client;

namespace Backend.Validation;

public class StartGameValidator : AbstractValidator<MessageContext<StartGameClientMessage>>
{
    public StartGameValidator(IRoomManager roomManager)
    {
        RuleFor(x => x.Sender)
            .MustBeRoomOwner(gr => gr is WaitingGameState, roomManager);
    }
}
