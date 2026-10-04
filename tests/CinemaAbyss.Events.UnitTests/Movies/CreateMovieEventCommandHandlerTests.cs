using CinemaAbyss.Events.Application.Common.Interfaces;
using CinemaAbyss.Events.Application.Common.Models;
using CinemaAbyss.Events.Application.Movies;
using CinemaAbyss.Events.Domain.Events;
using Moq;

namespace CinemaAbyss.Events.UnitTests.Movies;

public class CreateMovieEventCommandHandlerTests
{
    [Fact]
    public async Task Handle_PublishesMovieEventAndReturnsSuccessResponse()
    {
        EventEnvelope? published = null;
        var publisher = new Mock<IEventPublisher>();
        publisher.Setup(p => p.PublishAsync(It.IsAny<EventEnvelope>(), It.IsAny<CancellationToken>()))
            .Callback<EventEnvelope, CancellationToken>((envelope, _) => published = envelope)
            .ReturnsAsync(new PublishResult(Partition: 0, Offset: 42));

        var handler = new CreateMovieEventCommandHandler(publisher.Object);
        var command = new CreateMovieEventCommand(7, "Inception", "viewed", 3, null, null, null);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("success", response.Status);
        Assert.Equal(0, response.Partition);
        Assert.Equal(42, response.Offset);
        Assert.Equal("movie", response.Event.Type);

        Assert.NotNull(published);
        Assert.Equal("movie-events", published!.Topic);
        Assert.Equal("7", published.Key);
        Assert.IsType<MovieEventPayload>(published.Payload);
    }

    [Fact]
    public async Task Handle_PropagatesPublisherFailure()
    {
        var publisher = new Mock<IEventPublisher>();
        publisher.Setup(p => p.PublishAsync(It.IsAny<EventEnvelope>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("broker down"));

        var handler = new CreateMovieEventCommandHandler(publisher.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new CreateMovieEventCommand(1, "x", "viewed", null, null, null, null), CancellationToken.None));
    }
}
