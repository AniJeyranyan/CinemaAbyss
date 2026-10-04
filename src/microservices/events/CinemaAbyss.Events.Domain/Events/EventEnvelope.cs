namespace CinemaAbyss.Events.Domain.Events;

/// <summary>
/// A domain event ready to publish. <see cref="Key"/> decides the Kafka partition, so events for
/// the same movie, user or payment stay in order.
/// </summary>
public sealed class EventEnvelope
{
    private EventEnvelope(string id, string key, EventType type, DateTimeOffset timestamp, object payload)
    {
        Id = id;
        Key = key;
        Type = type;
        Timestamp = timestamp;
        Payload = payload;
    }

    public string Id { get; }
    public string Key { get; }
    public EventType Type { get; }
    public DateTimeOffset Timestamp { get; }
    public object Payload { get; }

    public string Topic => EventTopics.For(Type);

    /// <summary>Creates an envelope with a fresh unique id, so every published event is distinct.</summary>
    public static EventEnvelope Create(EventType type, DateTimeOffset timestamp, string key, object payload)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key is required.", nameof(key));

        return new EventEnvelope(Guid.NewGuid().ToString(), key, type, timestamp.ToUniversalTime(), payload);
    }
}
