using CinemaAbyss.Monolith.Domain.Entities;

namespace CinemaAbyss.Monolith.Application.Common.Interfaces;

public interface ISubscriptionRepository
{
    Task<IReadOnlyList<Subscription>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Subscription>> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<Subscription?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Subscription> AddAsync(Subscription subscription, CancellationToken cancellationToken);
}
