using CinemaAbyss.Monolith.Application.Common.DTOs;
using CinemaAbyss.Monolith.Application.Subscriptions.Commands;
using CinemaAbyss.Monolith.Application.Subscriptions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAbyss.Monolith.Api.Controllers;

[ApiController]
[Route("api/subscriptions")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISender _sender;

    public SubscriptionsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SubscriptionDto>>> GetAll(
        [FromQuery] int? id, [FromQuery] int? user_id, CancellationToken cancellationToken)
    {
        if (id.HasValue)
        {
            var subscription = await _sender.Send(new GetSubscriptionByIdQuery(id.Value), cancellationToken);
            return subscription is null ? NotFound() : Ok(subscription);
        }

        var subscriptions = await _sender.Send(new GetAllSubscriptionsQuery(user_id), cancellationToken);
        return Ok(subscriptions);
    }

    [HttpPost]
    public async Task<ActionResult<SubscriptionDto>> Create([FromBody] CreateSubscriptionRequest request, CancellationToken cancellationToken)
    {
        var subscription = await _sender.Send(
            new CreateSubscriptionCommand(request.UserId, request.PlanType, request.StartDate, request.EndDate),
            cancellationToken);
        return StatusCode(StatusCodes.Status201Created, subscription);
    }
}

public record CreateSubscriptionRequest(int UserId, string PlanType, DateTime StartDate, DateTime EndDate);
