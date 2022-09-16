using FluentValidation;
using Backend.Domain;
using Backend.Domain.Identifiers;
using Backend.Contracts.Messages;

namespace Backend.Services;

internal sealed class MessageValidator : IMessageValidator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MessageValidator> _logger;

    public MessageValidator(IServiceProvider serviceProvider, ILogger<MessageValidator> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public bool Validate(CID cid, IClientMessage message)
    {
        try
        {
            var messageType = message.GetType();
            var messageContextType = typeof(MessageContext<>).MakeGenericType(messageType);
            var validatorType = typeof(IValidator<>).MakeGenericType(messageContextType);

            var validator = (dynamic?)_serviceProvider.GetService(validatorType);
            if (validator is null)
            {
                _logger.LogWarning("No validation set up for {type}", messageType.Name);
                return true;
            }

            var messageContext = (dynamic?)Activator.CreateInstance(messageContextType, new object[] { cid, message });

            return validator.Validate(messageContext).IsValid;
        }
        catch
        {
            return false;
        }
    }
}
