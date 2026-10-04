namespace CinemaAbyss.Events.Application.Common.DTOs;

/// <summary>Response for a published event (EventResponse in the API spec).</summary>
public sealed record EventResponseDto(string Status, int Partition, long Offset, EventDto Event);

/// <summary>The published event as returned to the caller.</summary>
public sealed record EventDto(string Id, string Type, DateTimeOffset Timestamp, object Payload);
