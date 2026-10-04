using CinemaAbyss.Movies.Application.Common.Interfaces;
using CinemaAbyss.Movies.Infrastructure.Persistence;
using CinemaAbyss.Movies.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaAbyss.Movies.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
            ?? "Host=localhost;Database=cinemaabyss;Username=postgres;Password=postgres";

        services.AddDbContext<MoviesDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IMovieRepository, MovieRepository>();

        return services;
    }
}
