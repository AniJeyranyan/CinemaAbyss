using CinemaAbyss.Events.Application.Common.DTOs;
using CinemaAbyss.Events.Application.Common.Interfaces;
using CinemaAbyss.Events.Application.Common.Mappings;
using CinemaAbyss.Events.Domain.Events;
using MediatR;

namespace CinemaAbyss.Events.Application.Users;

public record CreateUserEventCommand(int UserId, string? Username, string? Email, string Action) : IRequest<EventResponseDto>;

public class CreateUserEventCommandHandler : IRequestHandler<CreateUserEventCommand, EventResponseDto>
{
    private readonly IEventPublisher _publisher;

    public CreateUserEventCommandHandler(IEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task<EventResponseDto> Handle(CreateUserEventCommand request, CancellationToken cancellationToken)
    {
        var payload = new UserEventPayload(request.UserId, request.Username, request.Email, request.Action);

        var envelope = EventEnvelope.CreateUserEvent(payload, DateTime.UtcNow);
        var result = await _publisher.PublishAsync(EventTopics.UserEvents, envelope, cancellationToken);

        return envelope.ToResponseDto(result);
    }
}
