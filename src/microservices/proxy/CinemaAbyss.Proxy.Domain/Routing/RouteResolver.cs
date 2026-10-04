namespace CinemaAbyss.Proxy.Domain.Routing;

/// <summary>
/// Pure Strangler Fig routing rules. The caller supplies a roll in [0, 99] so the
/// percentage decision stays deterministic and testable.
/// </summary>
public static class RouteResolver
{
    public static BackendService Resolve(string path, MigrationPolicy policy, int roll)
    {
        if (string.IsNullOrEmpty(path)) path = "/";

        if (path.Equals("/api/movies/health", StringComparison.OrdinalIgnoreCase))
            return BackendService.MoviesService;

        if (HasPrefix(path, "/api/movies"))
            return ResolveMoviesBackend(policy, roll);

        if (HasPrefix(path, "/api/events"))
            return BackendService.EventsService;

        // Users, payments, subscriptions and anything not extracted yet stay on the monolith.
        return BackendService.Monolith;
    }

    public static BackendService ResolveMoviesBackend(MigrationPolicy policy, int roll)
    {
        if (!policy.GradualMigrationEnabled)
            return BackendService.MoviesService;

        if (roll is < 0 or > 99)
            throw new ArgumentOutOfRangeException(nameof(roll), "Roll must be between 0 and 99.");

        return roll < policy.MoviesMigrationPercent ? BackendService.MoviesService : BackendService.Monolith;
    }

    // Matches "/api/movies" and "/api/movies/..." but not "/api/moviesfoo".
    private static bool HasPrefix(string path, string prefix) =>
        path.Equals(prefix, StringComparison.OrdinalIgnoreCase) ||
        path.StartsWith(prefix + "/", StringComparison.OrdinalIgnoreCase);
}
