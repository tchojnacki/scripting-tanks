using Backend.Domain.Identifiers;
using Backend.Contracts.Messages;

namespace Backend.Domain;

public record MessageContext<TMessage>(CID Sender, TMessage Message) where TMessage : IClientMessage;
