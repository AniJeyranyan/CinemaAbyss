namespace CinemaAbyss.Proxy.Domain.Routing;

/// <summary>The upstream services the gateway can forward a request to.</summary>
public enum BackendService
{
    Monolith,
    MoviesService,
    EventsService
}
