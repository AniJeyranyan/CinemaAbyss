using CinemaAbyss.Monolith.Application.Common.DTOs;
using CinemaAbyss.Monolith.Application.Common.Interfaces;
using CinemaAbyss.Monolith.Application.Common.Mappings;
using CinemaAbyss.Monolith.Domain.Entities;
using MediatR;

namespace CinemaAbyss.Monolith.Application.Movies.Commands;

public record CreateMovieCommand(string Title, string Description, double Rating, IReadOnlyCollection<string> Genres) : IRequest<MovieDto>;

public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, MovieDto>
{
    private readonly IMovieRepository _movieRepository;

    public CreateMovieCommandHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<MovieDto> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = Movie.Create(request.Title, request.Description, request.Rating, request.Genres);
        var created = await _movieRepository.AddAsync(movie, cancellationToken);
        return created.ToDto();
    }
}
