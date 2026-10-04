namespace CinemaAbyss.Events.Domain.Events;

public class EventEnvelope
{
    public string Id { get; }
    public EventType Type { get; }
    public DateTime Timestamp { get; }
    public object Payload { get; }

    public EventEnvelope(string id, EventType type, DateTime timestamp, object payload)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Id is required.", nameof(id));

        Id = id;
        Type = type;
        Timestamp = timestamp;
        Payload = payload;
    }

    public static EventEnvelope CreateMovieEvent(MovieEventPayload payload, DateTime timestamp) =>
        new($"movie-{payload.MovieId}-{payload.Action}", EventType.Movie, timestamp, payload);

    public static EventEnvelope CreateUserEvent(UserEventPayload payload, DateTime timestamp) =>
        new($"user-{payload.UserId}-{payload.Action}", EventType.User, timestamp, payload);

    public static EventEnvelope CreatePaymentEvent(PaymentEventPayload payload, DateTime timestamp) =>
        new($"payment-{payload.PaymentId}-{payload.Status}", EventType.Payment, timestamp, payload);
}
