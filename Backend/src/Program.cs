using Microsoft.Extensions.FileProviders;
using Backend.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls($"http://*:{Environment.GetEnvironmentVariable("PORT") ?? "3000"}");

builder.Services.AddDirectoryBrowser();

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