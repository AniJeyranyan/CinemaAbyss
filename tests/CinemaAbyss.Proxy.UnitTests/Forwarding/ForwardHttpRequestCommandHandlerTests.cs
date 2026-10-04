using CinemaAbyss.Proxy.Application.Common.Interfaces;
using CinemaAbyss.Proxy.Application.Common.Models;
using CinemaAbyss.Proxy.Application.Forwarding;
using CinemaAbyss.Proxy.Domain.Routing;
using Moq;

namespace CinemaAbyss.Proxy.UnitTests.Forwarding;

public class ForwardHttpRequestCommandHandlerTests
{
    private static readonly ForwardRequest Request = new(
        "GET", "/api/movies", string.Empty, new Dictionary<string, string[]>(), [], null);

    private static readonly ForwardResponse Response = new(200, new Dictionary<string, string[]>(), [], "application/json");

    [Fact]
    public async Task Handle_ForwardsToTheBackendChosenByThePolicyAndRoll()
    {
        var policy = new Mock<IMigrationPolicyProvider>();
        policy.Setup(p => p.GetCurrentPolicy()).Returns(new MigrationPolicy(true, 50));

        var random = new Mock<IRandomProvider>();
        random.Setup(r => r.NextPercentRoll()).Returns(10);

        var urls = new Mock<IBackendUrlProvider>();
        urls.Setup(u => u.GetBaseUrl(BackendService.MoviesService)).Returns("http://movies-service:8081");

        var forwarder = new Mock<IRequestForwarder>();
        forwarder.Setup(f => f.ForwardAsync("http://movies-service:8081", Request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response);

        var handler = new ForwardHttpRequestCommandHandler(policy.Object, random.Object, urls.Object, forwarder.Object);

        var result = await handler.Handle(new ForwardHttpRequestCommand(Request), CancellationToken.None);

        Assert.Same(Response, result);
        forwarder.VerifyAll();
    }

    [Fact]
    public async Task Handle_RoutesToMonolithWhenRollIsAboveThePercentage()
    {
        var policy = new Mock<IMigrationPolicyProvider>();
        policy.Setup(p => p.GetCurrentPolicy()).Returns(new MigrationPolicy(true, 50));

        var random = new Mock<IRandomProvider>();
        random.Setup(r => r.NextPercentRoll()).Returns(80);

        var urls = new Mock<IBackendUrlProvider>();
        urls.Setup(u => u.GetBaseUrl(BackendService.Monolith)).Returns("http://monolith:8080");

        var forwarder = new Mock<IRequestForwarder>();
        forwarder.Setup(f => f.ForwardAsync("http://monolith:8080", Request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response);

        var handler = new ForwardHttpRequestCommandHandler(policy.Object, random.Object, urls.Object, forwarder.Object);

        await handler.Handle(new ForwardHttpRequestCommand(Request), CancellationToken.None);

        forwarder.VerifyAll();
    }
}
