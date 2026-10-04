using CinemaAbyss.Proxy.Application.Common.Interfaces;
using CinemaAbyss.Proxy.Domain.Routing;
using Microsoft.Extensions.Options;

namespace CinemaAbyss.Proxy.Infrastructure.Options;

public class MigrationPolicyProvider : IMigrationPolicyProvider
{
    private readonly ProxyOptions _options;

    public MigrationPolicyProvider(IOptions<ProxyOptions> options)
    {
        _options = options.Value;
    }

    public MigrationPolicy GetCurrentPolicy() =>
        new(_options.GradualMigration, _options.MoviesMigrationPercent);
}
