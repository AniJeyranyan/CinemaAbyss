using CinemaAbyss.Movies.Application.Common.DTOs;
using CinemaAbyss.Movies.Application.Common.Interfaces;
using CinemaAbyss.Movies.Application.Common.Mappings;
using MediatR;

namespace CinemaAbyss.Movies.Application.Movies.Queries;

public record GetAllMoviesQuery : IRequest<IReadOnlyList<MovieDto>>;

public class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, IReadOnlyList<MovieDto>>
{
    private readonly IMovieRepository _movieRepository;

    public GetAllMoviesQueryHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<IReadOnlyList<MovieDto>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var movies = await _movieRepository.GetAllAsync(cancellationToken);
        return movies.Select(m => m.ToDto()).ToList();
    }
}
