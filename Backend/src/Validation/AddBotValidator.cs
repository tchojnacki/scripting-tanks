using FluentValidation;
using Backend.Services;
using Backend.Domain;
using Backend.Domain.Rooms;
using Backend.Domain.Rooms.GameStates;
using Backend.Contracts.Messages.Client;

namespace Backend.Validation;

internal sealed class AddBotValidator : AbstractValidator<MessageContext<AddBotClientMessage>>
{
    public AddBotValidator(IRoomManager roomManager)
    {
        RuleFor(x => x.Sender).MustBeRoomOwner(
            gr => gr is WaitingGameState && gr.AllPlayers.Count() < GameRoom.MaxPlayers,
            roomManager);
    }
}
