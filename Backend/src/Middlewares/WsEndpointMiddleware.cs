using Backend.Domain.Identifiers;
using Backend.Services;

namespace Backend.Middlewares;

internal sealed class WsEndpointMiddleware
{
    private const string EndpointPath = "/ws";
    private readonly IConnectionManager _connectionManager;
    private readonly IHostApplicationLifetime _lifetime;

    private readonly RequestDelegate _next;

    public WsEndpointMiddleware(
        RequestDelegate next,
        IHostApplicationLifetime lifetime,
        IConnectionManager connectionManager)
    {
        _next = next;
        _lifetime = lifetime;
        _connectionManager = connectionManager;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == EndpointPath)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                using var socket = await context.WebSockets.AcceptWebSocketAsync();
                await _connectionManager.AcceptConnectionAsync(
                    Cid.FromConnection(context.Connection),
                    socket,
                    _lifetime.ApplicationStopping);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
        else
        {
            await _next(context);
        }
    }
}

public static class WsEndpointMiddlewareExtensions
{
    public static IApplicationBuilder UseWsEndpoint(this IApplicationBuilder builder)
        => builder.UseMiddleware<WsEndpointMiddleware>();
}