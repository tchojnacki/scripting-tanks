using System.Net.WebSockets;
using System.Text;

namespace Backend.Middlewares;

public class WsEndpointMiddleware
{
    private const string EndpointPath = "/ws";

    private readonly RequestDelegate _next;
    private readonly IHostApplicationLifetime _lifetime;

    public WsEndpointMiddleware(RequestDelegate next, IHostApplicationLifetime lifetime)
    {
        _next = next;
        _lifetime = lifetime;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == EndpointPath)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await HandleConnection(webSocket);
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

    private async Task HandleConnection(WebSocket webSocket)
    {
        var buffer = new byte[4096];
        WebSocketReceiveResult result;

        try
        {
            do
            {
                Array.Clear(buffer);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _lifetime.ApplicationStopping);
                var text = Encoding.UTF8.GetString(buffer);
                Console.WriteLine(text);
            }
            while (!result.CloseStatus.HasValue);
        }
        catch
        {
        }
        finally
        {
            if (webSocket.State is not (WebSocketState.Closed or WebSocketState.Aborted))
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, _lifetime.ApplicationStopping);
            }
        }
    }
}

public static class WsEndpointMiddlewareExtensions
{
    public static IApplicationBuilder UseWsEndpoint(this IApplicationBuilder builder)
        => builder.UseMiddleware<WsEndpointMiddleware>();
}
