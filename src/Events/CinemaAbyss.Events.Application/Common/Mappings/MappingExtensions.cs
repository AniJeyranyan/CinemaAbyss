using CinemaAbyss.Events.Application.Common.DTOs;
using CinemaAbyss.Events.Application.Common.Models;
using CinemaAbyss.Events.Domain.Events;

namespace CinemaAbyss.Events.Application.Common.Mappings;

public static class MappingExtensions
{
    public static EventResponseDto ToResponseDto(this EventEnvelope envelope, PublishResult result) =>
        new(
            "success",
            result.Partition,
            result.Offset,
            new EventDto(envelope.Id, envelope.Type.ToString().ToLowerInvariant(), envelope.Timestamp, envelope.Payload));
}
