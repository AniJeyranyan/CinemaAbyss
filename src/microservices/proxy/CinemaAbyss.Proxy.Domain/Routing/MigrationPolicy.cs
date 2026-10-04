namespace CinemaAbyss.Proxy.Domain.Routing;

/// <summary>
/// Strangler Fig feature flag. When gradual migration is enabled, <see cref="MoviesMigrationPercent"/>
/// percent of /api/movies traffic goes to movies-service and the rest stays on the monolith.
/// </summary>
public sealed class MigrationPolicy
{
    public MigrationPolicy(bool gradualMigrationEnabled, int moviesMigrationPercent)
    {
        if (moviesMigrationPercent < 0 || moviesMigrationPercent > 100)
            throw new ArgumentOutOfRangeException(nameof(moviesMigrationPercent), "Percent must be between 0 and 100.");

        GradualMigrationEnabled = gradualMigrationEnabled;
        MoviesMigrationPercent = moviesMigrationPercent;
    }

    public bool GradualMigrationEnabled { get; }

    public int MoviesMigrationPercent { get; }
}
