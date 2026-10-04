using CinemaAbyss.SharedKernel;

namespace CinemaAbyss.Monolith.Domain.Entities;

public class Subscription : Entity<int>
{
    public int UserId { get; private set; }
    public string PlanType { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    private Subscription() { }

    // Validation is left to the database, as in the Go monolith; in particular Go
    // accepts start_date == end_date, which the Postman collection sends.
    public static Subscription Create(int userId, string planType, DateTime startDate, DateTime endDate)
    {
        return new Subscription
        {
            UserId = userId,
            PlanType = planType,
            StartDate = startDate,
            EndDate = endDate
        };
    }

    public static Subscription Reconstitute(int id, int userId, string planType, DateTime startDate, DateTime endDate) =>
        new()
        {
            Id = id,
            UserId = userId,
            PlanType = planType,
            StartDate = startDate,
            EndDate = endDate
        };
}
