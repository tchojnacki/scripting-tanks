using FluentValidation;
using Backend.Services;
using Backend.Domain;
using Backend.Domain.Rooms.GameStates;
using Backend.Domain.Identifiers;
using Backend.Contracts.Messages.Client;

namespace Backend.Validation;

internal sealed class PromotePlayerValidator : AbstractValidator<MessageContext<PromotePlayerClientMessage>>
{
    public PromotePlayerValidator(IConnectionManager connectionManager, IRoomManager roomManager)
    {
        Transform(from: x => x.Message.Data, to: CID.Deserialize)
            .MustBePlayer(p => !p.IsBot, connectionManager)
            .MustBeInGameRoom((gr, _, x) => gr is WaitingGameState && x.Sender == gr.OwnerCID, roomManager);
    }
}
