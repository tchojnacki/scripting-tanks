using FluentValidation;
using Backend.Services;
using Backend.Domain;
using Backend.Domain.Rooms.States;
using Backend.Contracts.Messages.Client;

namespace Backend.Validation;

public class AddBotValidator : AbstractValidator<MessageContext<AddBotClientMessage>>
{
    public AddBotValidator(IRoomManager roomManager)
    {
        RuleFor(x => x.Sender).MustBeRoomOwner(gr => gr.State is WaitingGameState, roomManager);
    }
}