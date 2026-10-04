using CinemaAbyss.Proxy.Application.Common.Interfaces;
using CinemaAbyss.Proxy.Application.Common.Models;
using CinemaAbyss.Proxy.Application.Forwarding;
using CinemaAbyss.Proxy.Domain.Routing;
using Moq;
using Xunit;

namespace CinemaAbyss.Proxy.UnitTests.Forwarding;

public class ForwardHttpRequestCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_ForwardToMoviesService_When_PathIsMoviesHealth()
    {
        var policyProvider = new Mock<IMigrationPolicyProvider>();
        policyProvider.Setup(p => p.GetCurrentPolicy()).Returns(new MigrationPolicy(true, 0));

        var randomProvider = new Mock<IRandomProvider>();
        randomProvider.Setup(r => r.NextPercentRoll()).Returns(0);

        var urlProvider = new Mock<IBackendUrlProvider>();
        urlProvider.Setup(p => p.GetBaseUrl(BackendService.MoviesService)).Returns("http://movies-service:8081");

        var expectedResponse = new ForwardResponse(200, new Dictionary<string, string[]>(), Array.Empty<byte>(), "application/json");
        var forwarder = new Mock<IRequestForwarder>();
        forwarder
            .Setup(f => f.ForwardAsync("http://movies-service:8081", It.IsAny<ForwardRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var handler = new ForwardHttpRequestCommandHandler(policyProvider.Object, randomProvider.Object, urlProvider.Object, forwarder.Object);

        var request = new ForwardRequest("GET", "/api/movies/health", "", new Dictionary<string, string[]>(), Array.Empty<byte>(), null);
        var response = await handler.Handle(new ForwardHttpRequestCommand(request), CancellationToken.None);

        Assert.Same(expectedResponse, response);
        urlProvider.Verify(p => p.GetBaseUrl(BackendService.MoviesService), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_RouteToMonolith_When_GradualMigrationDisabled_And_PathIsUsers()
    {
        var policyProvider = new Mock<IMigrationPolicyProvider>();
        policyProvider.Setup(p => p.GetCurrentPolicy()).Returns(new MigrationPolicy(false, 100));

        var randomProvider = new Mock<IRandomProvider>();
        randomProvider.Setup(r => r.NextPercentRoll()).Returns(0);

        var urlProvider = new Mock<IBackendUrlProvider>();
        urlProvider.Setup(p => p.GetBaseUrl(BackendService.Monolith)).Returns("http://monolith:8080");

        var expectedResponse = new ForwardResponse(200, new Dictionary<string, string[]>(), Array.Empty<byte>(), "application/json");
        var forwarder = new Mock<IRequestForwarder>();
        forwarder
            .Setup(f => f.ForwardAsync("http://monolith:8080", It.IsAny<ForwardRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var handler = new ForwardHttpRequestCommandHandler(policyProvider.Object, randomProvider.Object, urlProvider.Object, forwarder.Object);

        var request = new ForwardRequest("GET", "/api/users", "", new Dictionary<string, string[]>(), Array.Empty<byte>(), null);
        var response = await handler.Handle(new ForwardHttpRequestCommand(request), CancellationToken.None);

        Assert.Same(expectedResponse, response);
        urlProvider.Verify(p => p.GetBaseUrl(BackendService.Monolith), Times.Once);
    }
}
