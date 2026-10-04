namespace CinemaAbyss.Proxy.Domain.Routing;

/// <summary>
/// Strangler Fig feature flag controlling how /api/movies traffic is split between
/// the monolith and the movies microservice.
/// </summary>
public class MigrationPolicy
{
    public bool GradualMigrationEnabled { get; }
    public int MoviesMigrationPercent { get; }

    public MigrationPolicy(bool gradualMigrationEnabled, int moviesMigrationPercent)
    {
        if (moviesMigrationPercent < 0 || moviesMigrationPercent > 100)
            throw new ArgumentOutOfRangeException(nameof(moviesMigrationPercent), "Percent must be between 0 and 100.");

        GradualMigrationEnabled = gradualMigrationEnabled;
        MoviesMigrationPercent = moviesMigrationPercent;
    }
}
