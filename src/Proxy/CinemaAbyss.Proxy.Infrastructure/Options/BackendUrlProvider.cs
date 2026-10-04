using CinemaAbyss.Proxy.Application.Common.Interfaces;
using CinemaAbyss.Proxy.Domain.Routing;
using Microsoft.Extensions.Options;

namespace CinemaAbyss.Proxy.Infrastructure.Options;

public class BackendUrlProvider : IBackendUrlProvider
{
    private readonly ProxyOptions _options;

    public BackendUrlProvider(IOptions<ProxyOptions> options)
    {
        _options = options.Value;
    }

    public string GetBaseUrl(BackendService service) => service switch
    {
        BackendService.Monolith => _options.MonolithUrl,
        BackendService.MoviesService => _options.MoviesServiceUrl,
        BackendService.EventsService => _options.EventsServiceUrl,
        _ => throw new ArgumentOutOfRangeException(nameof(service), service, "Unknown backend service.")
    };
}
