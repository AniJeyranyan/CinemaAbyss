using System.ComponentModel.DataAnnotations;
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
        [FromBody] MovieEventRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new CreateMovieEventCommand(
                request.MovieId, request.Title!, request.Action!, request.UserId, request.Rating, request.Genres, request.Description),
            cancellationToken);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpPost("user")]
    public async Task<ActionResult<EventResponseDto>> CreateUserEvent(
        [FromBody] UserEventRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new CreateUserEventCommand(request.UserId, request.Username, request.Email, request.Action!, request.Timestamp!.Value),
            cancellationToken);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpPost("payment")]
    public async Task<ActionResult<EventResponseDto>> CreatePaymentEvent(
        [FromBody] PaymentEventRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new CreatePaymentEventCommand(
                request.PaymentId, request.UserId, request.Amount, request.Status!, request.MethodType, request.Timestamp!.Value),
            cancellationToken);
        return StatusCode(StatusCodes.Status201Created, response);
    }
}

// Request bodies. The [ApiController] attribute validates these before the action runs.

// Validation attributes sit on properties: ASP.NET Core ignores them on record primary-constructor parameters.

public class MovieEventRequest
{
    [Range(1, int.MaxValue)]
    public int MovieId { get; init; }

    [Required]
    public string? Title { get; init; }

    [Required]
    public string? Action { get; init; }

    public int? UserId { get; init; }
    public double? Rating { get; init; }
    public IReadOnlyCollection<string>? Genres { get; init; }
    public string? Description { get; init; }
}

public class UserEventRequest
{
    [Range(1, int.MaxValue)]
    public int UserId { get; init; }

    public string? Username { get; init; }
    public string? Email { get; init; }

    [Required]
    public string? Action { get; init; }

    [Required]
    public DateTimeOffset? Timestamp { get; init; }
}

public class PaymentEventRequest
{
    [Range(1, int.MaxValue)]
    public int PaymentId { get; init; }

    [Range(1, int.MaxValue)]
    public int UserId { get; init; }

    [Range(0, 1_000_000_000)]
    public decimal Amount { get; init; }

    [Required]
    public string? Status { get; init; }

    [Required]
    public DateTimeOffset? Timestamp { get; init; }

    public string? MethodType { get; init; }
}
