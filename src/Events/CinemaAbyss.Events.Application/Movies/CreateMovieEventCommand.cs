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

    public async Task<EventResponseDto> Handle(CreateMovieEventCommand request, CancellationToken cancellationToken)
    {
        var payload = new MovieEventPayload(
            request.MovieId, request.Title, request.Action, request.UserId, request.Rating, request.Genres, request.Description);

        var envelope = EventEnvelope.CreateMovieEvent(payload, DateTime.UtcNow);
        var result = await _publisher.PublishAsync(EventTopics.MovieEvents, envelope, cancellationToken);

        return envelope.ToResponseDto(result);
    }
}
