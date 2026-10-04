namespace CinemaAbyss.Movies.Application.Common.DTOs;

public record MovieDto(int Id, string Title, string Description, double Rating, IReadOnlyCollection<string> Genres);
