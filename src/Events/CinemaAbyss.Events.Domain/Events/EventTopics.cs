namespace CinemaAbyss.Events.Domain.Events;

public static class EventTopics
{
    public const string MovieEvents = "movie-events";
    public const string UserEvents = "user-events";
    public const string PaymentEvents = "payment-events";

    public static string For(EventType type) => type switch
    {
        EventType.Movie => MovieEvents,
        EventType.User => UserEvents,
        EventType.Payment => PaymentEvents,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown event type.")
    };
}
