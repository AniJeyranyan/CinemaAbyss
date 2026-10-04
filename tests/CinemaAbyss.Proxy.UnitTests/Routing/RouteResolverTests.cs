using CinemaAbyss.Proxy.Domain.Routing;
using Xunit;

namespace CinemaAbyss.Proxy.UnitTests.Routing;

public class RouteResolverTests
{
    [Fact]
    public void Resolve_Should_AlwaysRouteMoviesHealthToMoviesService()
    {
        var policy = new MigrationPolicy(gradualMigrationEnabled: true, moviesMigrationPercent: 0);

        var backend = RouteResolver.Resolve("/api/movies/health", policy, roll: 99);

        Assert.Equal(BackendService.MoviesService, backend);
    }

    [Fact]
    public void Resolve_Should_RouteUsersToMonolith()
    {
        var policy = new MigrationPolicy(gradualMigrationEnabled: true, moviesMigrationPercent: 100);

        var backend = RouteResolver.Resolve("/api/users", policy, roll: 0);

        Assert.Equal(BackendService.Monolith, backend);
    }

    [Fact]
    public void Resolve_Should_RouteEventsToEventsService()
    {
        var policy = new MigrationPolicy(gradualMigrationEnabled: false, moviesMigrationPercent: 0);

        var backend = RouteResolver.Resolve("/api/events/movie", policy, roll: 0);

        Assert.Equal(BackendService.EventsService, backend);
    }

    [Fact]
    public void ResolveMoviesBackend_Should_AlwaysUseMoviesService_When_GradualMigrationDisabled()
    {
        var policy = new MigrationPolicy(gradualMigrationEnabled: false, moviesMigrationPercent: 0);

        var backend = RouteResolver.ResolveMoviesBackend(policy, roll: 50);

        Assert.Equal(BackendService.MoviesService, backend);
    }

    [Theory]
    [InlineData(49, BackendService.MoviesService)]
    [InlineData(50, BackendService.Monolith)]
    [InlineData(99, BackendService.Monolith)]
    public void ResolveMoviesBackend_Should_SplitByPercent_When_GradualMigrationEnabled(int roll, BackendService expected)
    {
        var policy = new MigrationPolicy(gradualMigrationEnabled: true, moviesMigrationPercent: 50);

        var backend = RouteResolver.ResolveMoviesBackend(policy, roll);

        Assert.Equal(expected, backend);
    }

    [Fact]
    public void ResolveMoviesBackend_Should_Throw_When_RollOutOfRange()
    {
        var policy = new MigrationPolicy(gradualMigrationEnabled: true, moviesMigrationPercent: 50);

        Assert.Throws<ArgumentOutOfRangeException>(() => RouteResolver.ResolveMoviesBackend(policy, roll: 100));
    }
}
