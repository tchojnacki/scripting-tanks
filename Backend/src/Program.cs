using System.Runtime.CompilerServices;
using Backend;
using Backend.Middlewares;
using Microsoft.Extensions.FileProviders;

[assembly: InternalsVisibleTo("Backend.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

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
