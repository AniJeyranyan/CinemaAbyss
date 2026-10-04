using System.Net.Http.Headers;
using CinemaAbyss.Proxy.Application.Common.Interfaces;
using CinemaAbyss.Proxy.Application.Common.Models;

namespace CinemaAbyss.Proxy.Infrastructure.Http;

public class HttpRequestForwarder : IRequestForwarder
{
    // Headers that describe the client-to-gateway hop, or that HttpClient sets itself.
    private static readonly HashSet<string> RestrictedRequestHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Host", "Content-Length", "Transfer-Encoding", "Connection", "Content-Type"
    };

    private readonly IHttpClientFactory _httpClientFactory;

    public HttpRequestForwarder(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ForwardResponse> ForwardAsync(string baseUrl, ForwardRequest request, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient(nameof(HttpRequestForwarder));
        var uri = new Uri(new Uri(baseUrl), request.Path + request.QueryString);

        using var httpRequest = new HttpRequestMessage(new HttpMethod(request.Method), uri);

        if (request.Body.Length > 0)
        {
            httpRequest.Content = new ByteArrayContent(request.Body);
            if (!string.IsNullOrEmpty(request.ContentType))
                httpRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(request.ContentType);
        }

        foreach (var header in request.Headers)
        {
            if (RestrictedRequestHeaders.Contains(header.Key)) continue;
            httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        using var response = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseContentRead, cancellationToken);
        var body = await response.Content.ReadAsByteArrayAsync(cancellationToken);

        var headers = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
        foreach (var header in response.Headers)
            headers[header.Key] = header.Value.ToArray();
        foreach (var header in response.Content.Headers)
            headers[header.Key] = header.Value.ToArray();

        return new ForwardResponse(
            (int)response.StatusCode,
            headers,
            body,
            response.Content.Headers.ContentType?.ToString());
    }
}
