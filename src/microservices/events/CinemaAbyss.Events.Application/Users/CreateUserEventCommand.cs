using CinemaAbyss.Events.Application.Common.DTOs;
using CinemaAbyss.Events.Application.Common.Interfaces;
using CinemaAbyss.Events.Application.Common.Mappings;
using CinemaAbyss.Events.Domain.Events;
using MediatR;

namespace CinemaAbyss.Events.Application.Users;

public record CreateUserEventCommand(
    int UserId,
    string? Username,
    string? Email,
    string Action,
    DateTimeOffset Timestamp) : IRequest<EventResponseDto>;

public class CreateUserEventCommandHandler : IRequestHandler<CreateUserEventCommand, EventResponseDto>
{
    private readonly IEventPublisher _publisher;

    public CreateUserEventCommandHandler(IEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task<EventResponseDto> Handle(CreateUserEventCommand command, CancellationToken cancellationToken)
    {
        var payload = new UserEventPayload(command.UserId, command.Username, command.Email, command.Action);

        var envelope = EventEnvelope.Create(EventType.User, command.Timestamp, command.UserId.ToString(), payload);
        var result = await _publisher.PublishAsync(envelope, cancellationToken);

        return EventMappings.ToResponse(envelope, result);
    }
}
