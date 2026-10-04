namespace CinemaAbyss.Events.Application.Common.DTOs;

public record EventDto(string Id, string Type, DateTime Timestamp, object Payload);

public record EventResponseDto(string Status, int Partition, long Offset, EventDto Event);
