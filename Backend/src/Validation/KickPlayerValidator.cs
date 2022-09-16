using FluentValidation;
using Backend.Services;
using Backend.Domain;
using Backend.Domain.Identifiers;
using Backend.Domain.Rooms.GameStates;
using Backend.Contracts.Messages.Client;

namespace Backend.Validation;

public class KickPlayerValidator : AbstractValidator<MessageContext<KickPlayerClientMessage>>
{
    public KickPlayerValidator(IRoomManager roomManager)
    {
        Transform(from: x => x.Message.Data, to: CID.Deserialize)
            .MustBeInGameRoom((gr, _, x) => gr is WaitingGameState && x.Sender == gr.OwnerCID, roomManager);
    }
}
