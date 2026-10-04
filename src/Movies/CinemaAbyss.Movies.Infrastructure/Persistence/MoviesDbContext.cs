using CinemaAbyss.Movies.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaAbyss.Movies.Infrastructure.Persistence;

public class MoviesDbContext : DbContext
{
    public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options) { }

    public DbSet<Movie> Movies => Set<Movie>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MoviesDbContext).Assembly);
    }
}
