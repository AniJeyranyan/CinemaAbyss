using CinemaAbyss.Monolith.Application.Common.Interfaces;
using CinemaAbyss.Monolith.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaAbyss.Monolith.Infrastructure.Persistence.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly MonolithDbContext _dbContext;

    public MovieRepository(MonolithDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Movie>> GetAllAsync(CancellationToken cancellationToken) =>
        await _dbContext.Movies.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Movie?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        await _dbContext.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public async Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken)
    {
        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return movie;
    }
}
