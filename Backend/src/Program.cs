using System.Reflection;
using Microsoft.Extensions.FileProviders;
using MediatR;
using FluentValidation;
using Backend.Middlewares;
using Backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

builder.WebHost.UseUrls($"http://*:{Environment.GetEnvironmentVariable("PORT") ?? "3000"}");

builder.Services.AddDirectoryBrowser();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Transient);

builder.Services.AddSingleton<IRoomManager, RoomManager>();
builder.Services.AddSingleton<IConnectionManager, ConnectionManager>();
builder.Services.AddSingleton<IMessageSerializer, MessageSerializer>();
builder.Services.AddTransient<IMessageValidator, MessageValidator>();
builder.Services.AddTransient<IBroadcastHelper, BroadcastHelper>();
builder.Services.AddTransient<ICustomizationProvider, CustomizationProvider>();

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();

app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "../Client/dist")),
    EnableDirectoryBrowsing = true
});

app.UseWebSockets();
app.UseWsEndpoint();

app.Run();
