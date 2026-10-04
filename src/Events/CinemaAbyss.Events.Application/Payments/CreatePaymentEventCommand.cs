using CinemaAbyss.Events.Application.Common.DTOs;
using CinemaAbyss.Events.Application.Common.Interfaces;
using CinemaAbyss.Events.Application.Common.Mappings;
using CinemaAbyss.Events.Domain.Events;
using MediatR;

namespace CinemaAbyss.Events.Application.Payments;

public record CreatePaymentEventCommand(
    int PaymentId, int UserId, decimal Amount, string Status, string? MethodType) : IRequest<EventResponseDto>;

public class CreatePaymentEventCommandHandler : IRequestHandler<CreatePaymentEventCommand, EventResponseDto>
{
    private readonly IEventPublisher _publisher;

    public CreatePaymentEventCommandHandler(IEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task<EventResponseDto> Handle(CreatePaymentEventCommand request, CancellationToken cancellationToken)
    {
        var payload = new PaymentEventPayload(
            request.PaymentId, request.UserId, request.Amount, request.Status, request.MethodType);

        var envelope = EventEnvelope.CreatePaymentEvent(payload, DateTime.UtcNow);
        var result = await _publisher.PublishAsync(EventTopics.PaymentEvents, envelope, cancellationToken);

        return envelope.ToResponseDto(result);
    }
}
