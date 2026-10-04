using CinemaAbyss.Events.Application.Common.Models;
using CinemaAbyss.Events.Domain.Events;

namespace CinemaAbyss.Events.Application.Common.Interfaces;

/// <summary>Port for publishing domain events. The Infrastructure layer decides how (Kafka).</summary>
public interface IEventPublisher
{
    Task<PublishResult> PublishAsync(EventEnvelope envelope, CancellationToken cancellationToken);
}
