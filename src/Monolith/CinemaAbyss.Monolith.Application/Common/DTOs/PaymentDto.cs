namespace CinemaAbyss.Monolith.Application.Common.DTOs;

public record PaymentDto(int Id, int UserId, decimal Amount, DateTime Timestamp);
