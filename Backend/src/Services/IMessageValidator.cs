using Backend.Domain.Identifiers;
using Backend.Contracts.Messages;

namespace Backend.Services;

internal interface IMessageValidator
{
    bool Validate(CID cid, IClientMessage message);
}
