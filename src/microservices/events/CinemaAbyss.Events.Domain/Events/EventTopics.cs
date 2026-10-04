namespace CinemaAbyss.Events.Domain.Events;

/// <summary>Kafka topic per event type. Matches KAFKA_CREATE_TOPICS in docker-compose.yml.</summary>
public static class EventTopics
{
    public const string MovieEvents = "movie-events";
    public const string UserEvents = "user-events";
    public const string PaymentEvents = "payment-events";

    public static readonly IReadOnlyList<string> All = [MovieEvents, UserEvents, PaymentEvents];

    public static string For(EventType type) => type switch
    {
        EventType.Movie => MovieEvents,
        EventType.User => UserEvents,
        EventType.Payment => PaymentEvents,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown event type.")
    };
}
