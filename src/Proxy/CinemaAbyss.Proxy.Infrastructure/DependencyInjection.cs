using CinemaAbyss.Proxy.Application.Common.Interfaces;
using CinemaAbyss.Proxy.Infrastructure.Http;
using CinemaAbyss.Proxy.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaAbyss.Proxy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ProxyOptions>(configuration.GetSection(ProxyOptions.SectionName));

        services.AddHttpClient(nameof(HttpRequestForwarder));

        services.AddSingleton<IMigrationPolicyProvider, MigrationPolicyProvider>();
        services.AddSingleton<IBackendUrlProvider, BackendUrlProvider>();
        services.AddSingleton<IRandomProvider, RandomProvider>();
        services.AddScoped<IRequestForwarder, HttpRequestForwarder>();

        return services;
    }
}
