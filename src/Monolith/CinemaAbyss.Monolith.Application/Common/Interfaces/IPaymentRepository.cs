using CinemaAbyss.Monolith.Domain.Entities;

namespace CinemaAbyss.Monolith.Application.Common.Interfaces;

public interface IPaymentRepository
{
    Task<IReadOnlyList<Payment>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Payment>> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Payment> AddAsync(Payment payment, CancellationToken cancellationToken);
}
