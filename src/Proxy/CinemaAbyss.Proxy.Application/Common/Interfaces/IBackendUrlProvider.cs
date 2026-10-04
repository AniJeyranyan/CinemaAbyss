using CinemaAbyss.Proxy.Domain.Routing;

namespace CinemaAbyss.Proxy.Application.Common.Interfaces;

public interface IBackendUrlProvider
{
    string GetBaseUrl(BackendService service);
}
