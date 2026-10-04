using CinemaAbyss.Events.Application.Common.Interfaces;
using CinemaAbyss.Events.Application.Common.Models;
using CinemaAbyss.Events.Application.Users;
using CinemaAbyss.Events.Domain.Events;
using Moq;

namespace CinemaAbyss.Events.UnitTests.Users;

public class CreateUserEventCommandHandlerTests
{
    [Fact]
    public async Task Handle_UsesTheRequestTimestampAndKeysByUserId()
    {
        var timestamp = new DateTimeOffset(2023, 1, 15, 14, 30, 0, TimeSpan.Zero);
        EventEnvelope? published = null;

        var publisher = new Mock<IEventPublisher>();
        publisher.Setup(p => p.PublishAsync(It.IsAny<EventEnvelope>(), It.IsAny<CancellationToken>()))
            .Callback<EventEnvelope, CancellationToken>((envelope, _) => published = envelope)
            .ReturnsAsync(new PublishResult(1, 5));

        var handler = new CreateUserEventCommandHandler(publisher.Object);

        var response = await handler.Handle(
            new CreateUserEventCommand(9, "testuser", null, "logged_in", timestamp), CancellationToken.None);

        Assert.Equal("user", response.Event.Type);
        Assert.Equal(timestamp, response.Event.Timestamp);
        Assert.Equal("user-events", published!.Topic);
        Assert.Equal("9", published.Key);
    }
}
