using CinemaAbyss.Monolith.Application.Common.Interfaces;
using CinemaAbyss.Monolith.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaAbyss.Monolith.Infrastructure.Persistence.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly MonolithDbContext _dbContext;

    public SubscriptionRepository(MonolithDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Subscription>> GetAllAsync(CancellationToken cancellationToken) =>
        await _dbContext.Subscriptions.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Subscription>> GetByUserIdAsync(int userId, CancellationToken cancellationToken) =>
        await _dbContext.Subscriptions.AsNoTracking().Where(s => s.UserId == userId).ToListAsync(cancellationToken);

    public async Task<Subscription?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        await _dbContext.Subscriptions.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task<Subscription> AddAsync(Subscription subscription, CancellationToken cancellationToken)
    {
        _dbContext.Subscriptions.Add(subscription);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return subscription;
    }
}
