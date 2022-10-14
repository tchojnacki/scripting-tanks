using Backend.Contracts.Messages;
using Backend.Domain.Identifiers;

namespace Backend.Services;

internal interface IMessageValidator
{
    bool Validate(Cid cid, IClientMessage message);
}