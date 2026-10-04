using System.Text.Json;
using System.Text.Json.Serialization;

namespace CinemaAbyss.Events.Infrastructure.Kafka;

/// <summary>
/// Wire format of a message on a topic. Kept separate from the domain envelope so the broker
/// contract can change without touching the domain. Snake_case, the same as the HTTP API.
/// </summary>
public sealed record KafkaMessage(string Id, string Type, DateTimeOffset Timestamp, object Payload)
{
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}
