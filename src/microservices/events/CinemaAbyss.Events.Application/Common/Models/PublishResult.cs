namespace CinemaAbyss.Events.Application.Common.Models;

/// <summary>Where the broker stored a published event.</summary>
public sealed record PublishResult(int Partition, long Offset);
