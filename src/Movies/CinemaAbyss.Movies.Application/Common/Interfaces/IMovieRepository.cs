using CinemaAbyss.Movies.Domain.Entities;

namespace CinemaAbyss.Movies.Application.Common.Interfaces;

public interface IMovieRepository
{
    Task<IReadOnlyList<Movie>> GetAllAsync(CancellationToken cancellationToken);
    Task<Movie?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken);
}
