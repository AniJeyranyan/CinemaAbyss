using CinemaAbyss.Proxy.Domain.Routing;

namespace CinemaAbyss.Proxy.UnitTests.Routing;

public class RouteResolverTests
{
    private static readonly MigrationPolicy Gradual50 = new(gradualMigrationEnabled: true, moviesMigrationPercent: 50);
    private static readonly MigrationPolicy Disabled = new(gradualMigrationEnabled: false, moviesMigrationPercent: 50);

    [Fact]
    public void MoviesHealth_AlwaysGoesToMoviesService()
    {
        Assert.Equal(BackendService.MoviesService, RouteResolver.Resolve("/api/movies/health", Gradual50, 99));
        Assert.Equal(BackendService.MoviesService, RouteResolver.Resolve("/api/movies/health", Disabled, 99));
    }

    [Theory]
    [InlineData(0, BackendService.MoviesService)]
    [InlineData(49, BackendService.MoviesService)]
    [InlineData(50, BackendService.Monolith)]
    [InlineData(99, BackendService.Monolith)]
    public void MoviesTraffic_IsSplitByRoll(int roll, BackendService expected)
    {
        Assert.Equal(expected, RouteResolver.Resolve("/api/movies", Gradual50, roll));
    }

    [Fact]
    public void ZeroAndHundredPercent_SendEverythingToOneSide()
    {
        var none = new MigrationPolicy(true, 0);
        var all = new MigrationPolicy(true, 100);

        foreach (var roll in new[] { 0, 50, 99 })
        {
            Assert.Equal(BackendService.Monolith, RouteResolver.Resolve("/api/movies", none, roll));
            Assert.Equal(BackendService.MoviesService, RouteResolver.Resolve("/api/movies", all, roll));
        }
    }

    [Fact]
    public void DisabledMigration_SendsAllMoviesTrafficToMoviesService()
    {
        Assert.Equal(BackendService.MoviesService, RouteResolver.Resolve("/api/movies", Disabled, 99));
    }

    [Fact]
    public void Events_GoToEventsService()
    {
        Assert.Equal(BackendService.EventsService, RouteResolver.Resolve("/api/events/movie", Gradual50, 0));
        Assert.Equal(BackendService.EventsService, RouteResolver.Resolve("/api/events/health", Gradual50, 0));
    }

    [Theory]
    [InlineData("/api/users")]
    [InlineData("/api/payments")]
    [InlineData("/api/subscriptions")]
    [InlineData("/something/else")]
    public void EverythingElse_StaysOnTheMonolith(string path)
    {
        Assert.Equal(BackendService.Monolith, RouteResolver.Resolve(path, Gradual50, 0));
    }

    [Theory]
    [InlineData("/api/moviesx")]
    [InlineData("/api/eventsfoo")]
    public void PathsThatOnlyStartWithARouteName_AreNotRoutedByThatRule(string path)
    {
        Assert.Equal(BackendService.Monolith, RouteResolver.Resolve(path, Gradual50, 0));
    }

    [Theory]
    [InlineData(100)]
    [InlineData(-1)]
    public void RollOutsideRange_IsRejected(int roll)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RouteResolver.Resolve("/api/movies", Gradual50, roll));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void PercentOutsideRange_IsRejectedByPolicy(int percent)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new MigrationPolicy(true, percent));
    }
}
