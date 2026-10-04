using CinemaAbyss.Monolith.Application.Common.DTOs;
using CinemaAbyss.Monolith.Application.Movies.Commands;
using CinemaAbyss.Monolith.Application.Movies.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAbyss.Monolith.Api.Controllers;

[ApiController]
[Route("api/movies")]
public class MoviesController : ControllerBase
{
    private readonly ISender _sender;

    public MoviesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MovieDto>>> GetAll([FromQuery] int? id, CancellationToken cancellationToken)
    {
        if (id.HasValue)
        {
            var movie = await _sender.Send(new GetMovieByIdQuery(id.Value), cancellationToken);
            return movie is null ? NotFound() : Ok(movie);
        }

        var movies = await _sender.Send(new GetAllMoviesQuery(), cancellationToken);
        return Ok(movies);
    }

    [HttpPost]
    public async Task<ActionResult<MovieDto>> Create([FromBody] CreateMovieRequest request, CancellationToken cancellationToken)
    {
        var movie = await _sender.Send(
            new CreateMovieCommand(request.Title, request.Description, request.Rating, request.Genres ?? Array.Empty<string>()),
            cancellationToken);
        return StatusCode(StatusCodes.Status201Created, movie);
    }
}

public record CreateMovieRequest(string Title, string Description, double Rating, string[]? Genres);
