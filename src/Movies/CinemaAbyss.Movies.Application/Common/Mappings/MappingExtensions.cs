using CinemaAbyss.Movies.Application.Common.DTOs;
using CinemaAbyss.Movies.Domain.Entities;

namespace CinemaAbyss.Movies.Application.Common.Mappings;

public static class MappingExtensions
{
    public static MovieDto ToDto(this Movie movie) => new(movie.Id, movie.Title, movie.Description, movie.Rating, movie.Genres);
}
