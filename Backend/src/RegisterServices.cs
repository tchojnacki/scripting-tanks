using System.Reflection;
using MediatR;
using FluentValidation;
using Backend.Services;
using Backend.Domain;
using Backend.Contracts.Messages.Client;
using Backend.Validation;

internal static class RegisterServices
{
    public static void RegisterAll(this IServiceCollection services)
    {
        services.AddDirectoryBrowser();
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddSingleton<IRoomManager, RoomManager>();
        services.AddSingleton<IConnectionManager, ConnectionManager>();
        services.AddSingleton<IMessageSerializer, MessageSerializer>();
        services.AddTransient<IMessageValidator, MessageValidator>();
        services.AddTransient<IBroadcastHelper, BroadcastHelper>();
        services.AddTransient<ICustomizationProvider, CustomizationProvider>();

        services.AddTransient<IValidator<MessageContext<AddBotClientMessage>>, AddBotValidator>();
        services.AddTransient<IValidator<MessageContext<CloseLobbyClientMessage>>, CloseLobbyValidator>();
        services.AddTransient<IValidator<MessageContext<CreateLobbyClientMessage>>, CreateLobbyValidator>();
        services.AddTransient<IValidator<MessageContext<CustomizeColorsClientMessage>>, CustomizeColorsValidator>();
        services.AddTransient<IValidator<MessageContext<EnterLobbyClientMessage>>, EnterLobbyValidator>();
        services.AddTransient<IValidator<MessageContext<KickPlayerClientMessage>>, KickPlayerValidator>();
        services.AddTransient<IValidator<MessageContext<LeaveLobbyClientMessage>>, LeaveLobbyValidator>();
        services.AddTransient<IValidator<MessageContext<PromotePlayerClientMessage>>, PromotePlayerValidator>();
        services.AddTransient<IValidator<MessageContext<RerollNameClientMessage>>, RerollNameValidator>();
        services.AddTransient<IValidator<MessageContext<SetBarrelTargetClientMessage>>, SetBarrelTargetValidator>();
        services.AddTransient<IValidator<MessageContext<SetInputAxesClientMessage>>, SetInputAxesValidator>();
        services.AddTransient<IValidator<MessageContext<ShootClientMessage>>, ShootValidator>();
        services.AddTransient<IValidator<MessageContext<StartGameClientMessage>>, StartGameValidator>();
    }
}
