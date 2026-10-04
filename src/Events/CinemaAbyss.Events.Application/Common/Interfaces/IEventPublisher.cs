using CinemaAbyss.Events.Application.Common.Models;
using CinemaAbyss.Events.Domain.Events;

namespace CinemaAbyss.Events.Application.Common.Interfaces;

public interface IEventPublisher
{
    Task<PublishResult> PublishAsync(string topic, EventEnvelope envelope, CancellationToken cancellationToken);
}
