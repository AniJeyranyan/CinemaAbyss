using CinemaAbyss.Proxy.Application.Common.Models;
using CinemaAbyss.Proxy.Application.Forwarding;
using MediatR;

namespace CinemaAbyss.Proxy.Api.Middleware;

/// <summary>
/// Terminal middleware acting as the Strangler Fig gateway: everything except the
/// gateway's own /health check is forwarded to the backend chosen by the Application layer.
/// </summary>
public class ProxyForwardingMiddleware
{
    private static readonly HashSet<string> SkippedResponseHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Transfer-Encoding", "Content-Length", "Content-Type"
    };

    // UseMiddleware<T> requires a constructor taking the next RequestDelegate. This is
    // terminal middleware, so the pipeline ends here and `next` is never invoked.
    public ProxyForwardingMiddleware(RequestDelegate next)
    {
    }

    public async Task InvokeAsync(HttpContext context, ISender sender)
    {
        var path = context.Request.Path.Value ?? "/";

        if (path.Equals("/health", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Strangler Fig Proxy is healthy");
            return;
        }

        var body = await ReadBodyAsync(context.Request);
        var headers = context.Request.Headers.ToDictionary(
            h => h.Key,
            h => h.Value.Select(v => v ?? string.Empty).ToArray(),
            StringComparer.OrdinalIgnoreCase);

        var forwardRequest = new ForwardRequest(
            context.Request.Method,
            path,
            context.Request.QueryString.Value ?? string.Empty,
            headers,
            body,
            context.Request.ContentType);

        var response = await sender.Send(new ForwardHttpRequestCommand(forwardRequest), context.RequestAborted);

        context.Response.StatusCode = response.StatusCode;
        foreach (var header in response.Headers)
        {
            if (SkippedResponseHeaders.Contains(header.Key)) continue;
            context.Response.Headers[header.Key] = header.Value;
        }

        if (response.ContentType is not null)
            context.Response.ContentType = response.ContentType;

        await context.Response.Body.WriteAsync(response.Body);
    }

    private static async Task<byte[]> ReadBodyAsync(HttpRequest request)
    {
        if (!request.Body.CanSeek)
            request.EnableBuffering();

        request.Body.Position = 0;
        using var memoryStream = new MemoryStream();
        await request.Body.CopyToAsync(memoryStream);
        request.Body.Position = 0;
        return memoryStream.ToArray();
    }
}

public static class ProxyForwardingMiddlewareExtensions
{
    public static IApplicationBuilder UseProxyForwarding(this IApplicationBuilder app) =>
        app.UseMiddleware<ProxyForwardingMiddleware>();
}
