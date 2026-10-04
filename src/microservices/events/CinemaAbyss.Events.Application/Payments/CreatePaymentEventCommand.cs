using CinemaAbyss.Events.Application.Common.DTOs;
using CinemaAbyss.Events.Application.Common.Interfaces;
using CinemaAbyss.Events.Application.Common.Mappings;
using CinemaAbyss.Events.Domain.Events;
using MediatR;

namespace CinemaAbyss.Events.Application.Payments;

public record CreatePaymentEventCommand(
    int PaymentId,
    int UserId,
    decimal Amount,
    string Status,
    string? MethodType,
    DateTimeOffset Timestamp) : IRequest<EventResponseDto>;

public class CreatePaymentEventCommandHandler : IRequestHandler<CreatePaymentEventCommand, EventResponseDto>
{
    private readonly IEventPublisher _publisher;

    public CreatePaymentEventCommandHandler(IEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task<EventResponseDto> Handle(CreatePaymentEventCommand command, CancellationToken cancellationToken)
    {
        var payload = new PaymentEventPayload(command.PaymentId, command.UserId, command.Amount, command.Status, command.MethodType);

        var envelope = EventEnvelope.Create(EventType.Payment, command.Timestamp, command.PaymentId.ToString(), payload);
        var result = await _publisher.PublishAsync(envelope, cancellationToken);

        return EventMappings.ToResponse(envelope, result);
    }
}
