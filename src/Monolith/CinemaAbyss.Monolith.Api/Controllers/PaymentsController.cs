using CinemaAbyss.Monolith.Application.Common.DTOs;
using CinemaAbyss.Monolith.Application.Payments.Commands;
using CinemaAbyss.Monolith.Application.Payments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAbyss.Monolith.Api.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly ISender _sender;

    public PaymentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PaymentDto>>> GetAll(
        [FromQuery] int? id, [FromQuery] int? user_id, CancellationToken cancellationToken)
    {
        if (id.HasValue)
        {
            var payment = await _sender.Send(new GetPaymentByIdQuery(id.Value), cancellationToken);
            return payment is null ? NotFound() : Ok(payment);
        }

        var payments = await _sender.Send(new GetAllPaymentsQuery(user_id), cancellationToken);
        return Ok(payments);
    }

    [HttpPost]
    public async Task<ActionResult<PaymentDto>> Create([FromBody] CreatePaymentRequest request, CancellationToken cancellationToken)
    {
        var payment = await _sender.Send(new CreatePaymentCommand(request.UserId, request.Amount), cancellationToken);
        return StatusCode(StatusCodes.Status201Created, payment);
    }
}

public record CreatePaymentRequest(int UserId, decimal Amount);
