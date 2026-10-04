using CinemaAbyss.Monolith.Application.Common.Interfaces;
using CinemaAbyss.Monolith.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaAbyss.Monolith.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MonolithDbContext _dbContext;

    public UserRepository(MonolithDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken) =>
        await _dbContext.Users.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }
}
