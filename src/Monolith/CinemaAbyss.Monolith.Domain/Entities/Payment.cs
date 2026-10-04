using CinemaAbyss.SharedKernel;

namespace CinemaAbyss.Monolith.Domain.Entities;

public class Payment : Entity<int>
{
    public int UserId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Timestamp { get; private set; }

    private Payment() { }

    // Validation is left to the database, as in the Go monolith.
    public static Payment Create(int userId, decimal amount)
    {
        return new Payment
        {
            UserId = userId,
            Amount = amount,
            Timestamp = DateTime.UtcNow
        };
    }

    public static Payment Reconstitute(int id, int userId, decimal amount, DateTime timestamp) =>
        new()
        {
            Id = id,
            UserId = userId,
            Amount = amount,
            Timestamp = timestamp
        };
}
