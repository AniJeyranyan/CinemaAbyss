namespace CinemaAbyss.Monolith.Application.Common.DTOs;

public record SubscriptionDto(int Id, int UserId, string PlanType, DateTime StartDate, DateTime EndDate);
