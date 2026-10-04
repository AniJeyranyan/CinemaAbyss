using CinemaAbyss.Events.Application.Common.DTOs;
using CinemaAbyss.Events.Application.Common.Models;
using CinemaAbyss.Events.Domain.Events;

namespace CinemaAbyss.Events.Application.Common.Mappings;

public static class EventMappings
{
    /// <summary>Builds the response a caller receives once the broker has accepted the event.</summary>
    public static EventResponseDto ToResponse(EventEnvelope envelope, PublishResult result) =>
        new(
            Status: "success",
            Partition: result.Partition,
            Offset: result.Offset,
            Event: new EventDto(envelope.Id, envelope.Type.ToString().ToLowerInvariant(), envelope.Timestamp, envelope.Payload));
}
