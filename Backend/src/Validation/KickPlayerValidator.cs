using Backend.Contracts.Messages.Client;
using Backend.Domain;
using Backend.Domain.Identifiers;
using Backend.Domain.Rooms.GameStates;
using Backend.Services;
using FluentValidation;

namespace Backend.Validation;

internal sealed class KickPlayerValidator : AbstractValidator<MessageContext<KickPlayerClientMessage>>
{
    public KickPlayerValidator(IRoomManager roomManager)
    {
        Transform(x => x.Message.Data, Cid.Deserialize)
            .MustBeInGameRoom((gr, _, x) => gr is WaitingGameState && x.Sender == gr.OwnerCid, roomManager);
    }
}