using CinemaAbyss.Events.Application.Common.DTOs;
using CinemaAbyss.Events.Application.Movies;
using CinemaAbyss.Events.Application.Payments;
using CinemaAbyss.Events.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAbyss.Events.Api.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly ISender _sender;

    public EventsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = true });

    [HttpPost("movie")]
    public async Task<ActionResult<EventResponseDto>> CreateMovieEvent(
        [FromBody] CreateMovieEventRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new CreateMovieEventCommand(
                request.MovieId, request.Title, request.Action, request.UserId, request.Rating, request.Genres, request.Description),
            cancellationToken);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpPost("user")]
    public async Task<ActionResult<EventResponseDto>> CreateUserEvent(
        [FromBody] CreateUserEventRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new CreateUserEventCommand(request.UserId, request.Username, request.Email, request.Action),
            cancellationToken);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpPost("payment")]
    public async Task<ActionResult<EventResponseDto>> CreatePaymentEvent(
        [FromBody] CreatePaymentEventRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new CreatePaymentEventCommand(request.PaymentId, request.UserId, request.Amount, request.Status, request.MethodType),
            cancellationToken);
        return StatusCode(StatusCodes.Status201Created, response);
    }
}

public record CreateMovieEventRequest(
    int MovieId, string Title, string Action, int? UserId, double? Rating, string[]? Genres, string? Description);

public record CreateUserEventRequest(int UserId, string? Username, string? Email, string Action);

public record CreatePaymentEventRequest(int PaymentId, int UserId, decimal Amount, string Status, string? MethodType);
