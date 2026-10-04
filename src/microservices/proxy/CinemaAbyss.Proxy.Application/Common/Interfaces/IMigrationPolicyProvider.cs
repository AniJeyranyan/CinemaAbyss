using CinemaAbyss.Proxy.Domain.Routing;

namespace CinemaAbyss.Proxy.Application.Common.Interfaces;

public interface IMigrationPolicyProvider
{
    MigrationPolicy GetCurrentPolicy();
}
