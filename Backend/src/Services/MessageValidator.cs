using Backend.Contracts.Messages;
using Backend.Domain;
using Backend.Domain.Identifiers;
using FluentValidation;

namespace Backend.Services;

internal sealed class MessageValidator : IMessageValidator
{
    private readonly ILogger<MessageValidator> _logger;
    private readonly IServiceProvider _serviceProvider;

    public MessageValidator(IServiceProvider serviceProvider, ILogger<MessageValidator> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public bool Validate(Cid cid, IClientMessage message)
    {
        try
        {
            var messageType = message.GetType();
            var messageContextType = typeof(MessageContext<>).MakeGenericType(messageType);
            var validatorType = typeof(IValidator<>).MakeGenericType(messageContextType);

            var validator = (dynamic?)_serviceProvider.GetService(validatorType);
            if (validator is null)
            {
                _logger.LogWarning("No validation set up for {Type}", messageType.Name);
                return true;
            }

            var messageContext = (dynamic?)Activator.CreateInstance(messageContextType, cid, message);

            return validator.Validate(messageContext).IsValid;
        }
        catch
        {
            return false;
        }
    }
}