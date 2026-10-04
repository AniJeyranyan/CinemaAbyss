using CinemaAbyss.Monolith.Application.Common.DTOs;
using CinemaAbyss.Monolith.Domain.Entities;

namespace CinemaAbyss.Monolith.Application.Common.Mappings;

public static class MappingExtensions
{
    public static UserDto ToDto(this User user) => new(user.Id, user.Username, user.Email);

    public static MovieDto ToDto(this Movie movie) => new(movie.Id, movie.Title, movie.Description, movie.Rating, movie.Genres);

    public static PaymentDto ToDto(this Payment payment) => new(payment.Id, payment.UserId, payment.Amount, payment.Timestamp);

    public static SubscriptionDto ToDto(this Subscription subscription) =>
        new(subscription.Id, subscription.UserId, subscription.PlanType, subscription.StartDate, subscription.EndDate);
}
