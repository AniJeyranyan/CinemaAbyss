using CinemaAbyss.Events.Application.Common.DTOs;
using CinemaAbyss.Events.Application.Common.Interfaces;
using CinemaAbyss.Events.Application.Common.Mappings;
using CinemaAbyss.Events.Domain.Events;
using MediatR;

namespace CinemaAbyss.Events.Application.Movies;

public record CreateMovieEventCommand(
    int MovieId,
    string Title,
    string Action,
    int? UserId,
    double? Rating,
    IReadOnlyCollection<string>? Genres,
    string? Description) : IRequest<EventResponseDto>;

public class CreateMovieEventCommandHandler : IRequestHandler<CreateMovieEventCommand, EventResponseDto>
{
    private readonly IEventPublisher _publisher;

    public CreateMovieEventCommandHandler(IEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task<EventResponseDto> Handle(CreateMovieEventCommand command, CancellationToken cancellationToken)
    {
        var payload = new MovieEventPayload(
            command.MovieId, command.Title, command.Action, command.UserId, command.Rating, command.Genres, command.Description);

        var envelope = EventEnvelope.Create(EventType.Movie, DateTimeOffset.UtcNow, command.MovieId.ToString(), payload);
        var result = await _publisher.PublishAsync(envelope, cancellationToken);

        return EventMappings.ToResponse(envelope, result);
    }
}
