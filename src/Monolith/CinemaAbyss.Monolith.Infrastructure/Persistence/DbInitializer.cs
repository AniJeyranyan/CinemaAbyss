using CinemaAbyss.Monolith.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaAbyss.Monolith.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task MigrateAndSeedAsync(MonolithDbContext dbContext, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);

        if (!await dbContext.Users.AnyAsync(cancellationToken))
        {
            dbContext.Users.AddRange(
                User.Create("user1", "user1@example.com"),
                User.Create("user2", "user2@example.com"),
                User.Create("user3", "user3@example.com"));
        }

        if (!await dbContext.Movies.AnyAsync(cancellationToken))
        {
            dbContext.Movies.AddRange(
                Movie.Create("The Shawshank Redemption",
                    "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                    9.3, new[] { "Drama" }),
                Movie.Create("The Godfather",
                    "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.",
                    9.2, new[] { "Crime", "Drama" }),
                Movie.Create("The Dark Knight",
                    "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.",
                    9.0, new[] { "Action", "Crime", "Drama" }),
                Movie.Create("Pulp Fiction",
                    "The lives of two mob hitmen, a boxer, a gangster and his wife, and a pair of diner bandits intertwine in four tales of violence and redemption.",
                    8.9, new[] { "Crime", "Drama" }),
                Movie.Create("Forrest Gump",
                    "The presidencies of Kennedy and Johnson, the Vietnam War, the Watergate scandal and other historical events unfold from the perspective of an Alabama man with an IQ of 75, whose only desire is to be reunited with his childhood sweetheart.",
                    8.8, new[] { "Drama", "Romance" }));
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        if (!await dbContext.Payments.AnyAsync(cancellationToken))
        {
            var firstUserId = await dbContext.Users.OrderBy(u => u.Id).Select(u => u.Id).FirstAsync(cancellationToken);
            dbContext.Payments.Add(Payment.Create(firstUserId, 9.99m));
        }

        if (!await dbContext.Subscriptions.AnyAsync(cancellationToken))
        {
            var firstUserId = await dbContext.Users.OrderBy(u => u.Id).Select(u => u.Id).FirstAsync(cancellationToken);
            dbContext.Subscriptions.Add(Subscription.Create(firstUserId, "basic", DateTime.UtcNow, DateTime.UtcNow.AddDays(30)));
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
