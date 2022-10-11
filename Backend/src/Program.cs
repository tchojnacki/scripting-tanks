using System.Runtime.CompilerServices;
using Microsoft.Extensions.FileProviders;
using Backend.Middlewares;

[assembly: InternalsVisibleTo("Backend.Tests")]

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

builder.WebHost.UseUrls($"http://*:{Environment.GetEnvironmentVariable("PORT") ?? "3000"}");

builder.Services.RegisterAll();

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
