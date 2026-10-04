namespace CinemaAbyss.Proxy.Infrastructure.Options;

/// <summary>Gateway settings. Bound from the "Proxy" configuration section; the API layer maps environment variables onto it.</summary>
public class ProxyOptions
{
    public const string SectionName = "Proxy";

    public string MonolithUrl { get; set; } = "http://localhost:8080";
    public string MoviesServiceUrl { get; set; } = "http://localhost:8081";
    public string EventsServiceUrl { get; set; } = "http://localhost:8082";
    public bool GradualMigration { get; set; } = true;
    public int MoviesMigrationPercent { get; set; } = 50;
}
