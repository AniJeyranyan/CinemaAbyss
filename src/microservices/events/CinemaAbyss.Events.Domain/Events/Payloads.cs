namespace CinemaAbyss.Events.Domain.Events;

public record MovieEventPayload(
    int MovieId,
    string Title,
    string Action,
    int? UserId,
    double? Rating,
    IReadOnlyCollection<string>? Genres,
    string? Description);

public record UserEventPayload(
    int UserId,
    string? Username,
    string? Email,
    string Action);

public record PaymentEventPayload(
    int PaymentId,
    int UserId,
    decimal Amount,
    string Status,
    string? MethodType);
