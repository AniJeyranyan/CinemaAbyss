using CinemaAbyss.Events.Domain.Events;

namespace CinemaAbyss.Events.UnitTests.Domain;

public class EventEnvelopeTests
{
    [Fact]
    public void Create_GivesEachEventAUniqueId()
    {
        var payload = new UserEventPayload(1, null, null, "logged_in");
        var now = DateTimeOffset.UtcNow;

        var a = EventEnvelope.Create(EventType.User, now, "1", payload);
        var b = EventEnvelope.Create(EventType.User, now, "1", payload);

        Assert.NotEqual(a.Id, b.Id);
    }

    [Fact]
    public void Create_RequiresAKey()
    {
        Assert.Throws<ArgumentException>(() =>
            EventEnvelope.Create(EventType.Movie, DateTimeOffset.UtcNow, " ", new MovieEventPayload(1, "x", "viewed", null, null, null, null)));
    }

    [Theory]
    [InlineData(EventType.Movie, "movie-events")]
    [InlineData(EventType.User, "user-events")]
    [InlineData(EventType.Payment, "payment-events")]
    public void Topic_IsDerivedFromTheEventType(EventType type, string expectedTopic)
    {
        var envelope = EventEnvelope.Create(type, DateTimeOffset.UtcNow, "1", new object());

        Assert.Equal(expectedTopic, envelope.Topic);
    }
}
