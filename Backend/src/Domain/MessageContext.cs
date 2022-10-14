using Backend.Contracts.Messages;
using Backend.Domain.Identifiers;

namespace Backend.Domain;

internal sealed record MessageContext<TMessage>(Cid Sender, TMessage Message) where TMessage : IClientMessage;