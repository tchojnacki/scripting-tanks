using Backend.Contracts.Messages.Client;
using Backend.Domain;
using Backend.Domain.Identifiers;
using Backend.Domain.Rooms;
using Backend.Services;
using FluentValidation;

namespace Backend.Validation;

internal sealed class EnterLobbyValidator : AbstractValidator<MessageContext<EnterLobbyClientMessage>>
{
    public EnterLobbyValidator(IRoomManager roomManager)
    {
        RuleFor(x => x.Sender).MustBeInMenu(roomManager);
        Transform(x => x.Message.Data, Lid.Deserialize).MustBeAValidGameRoom(
            gr => gr.AllPlayers.Count() < GameRoom.MaxPlayers,
            roomManager);
    }
}