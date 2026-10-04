using CinemaAbyss.SharedKernel;

namespace CinemaAbyss.Monolith.Domain.Entities;

public class User : Entity<int>
{
    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private User() { }

    // Validation is left to the database, as in the Go monolith: the handler inserts
    // whatever the client sent and surfaces any constraint violation.
    public static User Create(string username, string email)
    {
        return new User
        {
            Username = username,
            Email = email,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static User Reconstitute(int id, string username, string email, DateTime createdAt) =>
        new()
        {
            Id = id,
            Username = username,
            Email = email,
            CreatedAt = createdAt
        };
}
