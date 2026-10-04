using CinemaAbyss.Events.Application.Common.Interfaces;
using CinemaAbyss.Events.Application.Common.Models;
using CinemaAbyss.Events.Application.Movies;
using CinemaAbyss.Events.Domain.Events;
using Moq;
using Xunit;

namespace CinemaAbyss.Events.UnitTests;

public class CreateMovieEventCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_PublishToMovieEventsTopic_And_ReturnResponse()
    {
        var publisher = new Mock<IEventPublisher>();
        publisher
            .Setup(p => p.PublishAsync(EventTopics.MovieEvents, It.IsAny<EventEnvelope>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PublishResult(0, 42));

        var handler = new CreateMovieEventCommandHandler(publisher.Object);

        var command = new CreateMovieEventCommand(1, "Inception", "viewed", UserId: 1, Rating: 8.5, Genres: new[] { "Sci-Fi" }, Description: null);
        var response = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("success", response.Status);
        Assert.Equal(0, response.Partition);
        Assert.Equal(42, response.Offset);
        Assert.Equal("movie-1-viewed", response.Event.Id);
        Assert.Equal("movie", response.Event.Type);

        publisher.Verify(
            p => p.PublishAsync(EventTopics.MovieEvents, It.IsAny<EventEnvelope>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
