namespace CinemaAbyss.Proxy.Domain.Routing;

/// <summary>
/// Pure Strangler Fig routing rules. The caller supplies a 0-99 "roll" so the
/// percentage-based decision stays deterministic and unit-testable.
/// </summary>
public static class RouteResolver
{
    public static BackendService Resolve(string path, MigrationPolicy policy, int roll)
    {
        if (string.IsNullOrEmpty(path)) path = "/";

        if (path.Equals("/api/movies/health", StringComparison.OrdinalIgnoreCase))
            return BackendService.MoviesService;

        if (path.StartsWith("/api/movies", StringComparison.OrdinalIgnoreCase))
            return ResolveMoviesBackend(policy, roll);

        if (path.StartsWith("/api/events", StringComparison.OrdinalIgnoreCase))
            return BackendService.EventsService;

        // users, payments, subscriptions and anything not yet extracted stay on the monolith.
        return BackendService.Monolith;
    }

    public static BackendService ResolveMoviesBackend(MigrationPolicy policy, int roll)
    {
        if (!policy.GradualMigrationEnabled)
            return BackendService.MoviesService;

        if (roll < 0 || roll > 99)
            throw new ArgumentOutOfRangeException(nameof(roll), "Roll must be between 0 and 99.");

        return roll < policy.MoviesMigrationPercent ? BackendService.MoviesService : BackendService.Monolith;
    }
}
