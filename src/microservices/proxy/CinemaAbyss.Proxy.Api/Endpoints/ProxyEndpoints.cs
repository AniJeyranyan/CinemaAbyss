using CinemaAbyss.Proxy.Application.Common.Models;
using CinemaAbyss.Proxy.Application.Forwarding;
using MediatR;

namespace CinemaAbyss.Proxy.Api.Endpoints;

public static class ProxyEndpoints
{
    // Response headers the gateway must not copy: ASP.NET Core frames the body itself,
    // and Content-Type is set once from the upstream response.
    private static readonly HashSet<string> SkippedResponseHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Transfer-Encoding", "Content-Length", "Content-Type"
    };

    public static void MapProxyEndpoints(this WebApplication app)
    {
        app.MapGet("/health", () => Results.Text("Strangler Fig Proxy is healthy", "text/plain"));

        // Everything except the gateway's own /health is forwarded to the backend chosen by the Application layer.
        app.Map("/{**path}", ForwardAsync);
    }

    private static async Task<IResult> ForwardAsync(HttpContext context, ISender sender, CancellationToken cancellationToken)
    {
        using var buffer = new MemoryStream();
        await context.Request.Body.CopyToAsync(buffer, cancellationToken);

        var headers = context.Request.Headers.ToDictionary(
            h => h.Key,
            h => h.Value.Select(v => v ?? string.Empty).ToArray(),
            StringComparer.OrdinalIgnoreCase);

        var request = new ForwardRequest(
            context.Request.Method,
            context.Request.Path.Value ?? "/",
            context.Request.QueryString.Value ?? string.Empty,
            headers,
            buffer.ToArray(),
            context.Request.ContentType);

        try
        {
            var response = await sender.Send(new ForwardHttpRequestCommand(request), cancellationToken);
            return new ForwardedResult(response);
        }
        catch (HttpRequestException)
        {
            return Results.Json(new { error = "Bad Gateway: backend is unavailable" }, statusCode: StatusCodes.Status502BadGateway);
        }
    }

    private sealed class ForwardedResult(ForwardResponse response) : IResult
    {
        public async Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = response.StatusCode;
            foreach (var header in response.Headers)
            {
                if (SkippedResponseHeaders.Contains(header.Key)) continue;
                httpContext.Response.Headers[header.Key] = header.Value;
            }

            if (response.ContentType is not null)
                httpContext.Response.ContentType = response.ContentType;

            await httpContext.Response.Body.WriteAsync(response.Body, httpContext.RequestAborted);
        }
    }
}
