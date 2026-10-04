using CinemaAbyss.Monolith.Application.Common.Interfaces;
using CinemaAbyss.Monolith.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaAbyss.Monolith.Infrastructure.Persistence.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly MonolithDbContext _dbContext;

    public PaymentRepository(MonolithDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Payment>> GetAllAsync(CancellationToken cancellationToken) =>
        await _dbContext.Payments.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Payment>> GetByUserIdAsync(int userId, CancellationToken cancellationToken) =>
        await _dbContext.Payments.AsNoTracking().Where(p => p.UserId == userId).ToListAsync(cancellationToken);

    public async Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        await _dbContext.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<Payment> AddAsync(Payment payment, CancellationToken cancellationToken)
    {
        _dbContext.Payments.Add(payment);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return payment;
    }
}
