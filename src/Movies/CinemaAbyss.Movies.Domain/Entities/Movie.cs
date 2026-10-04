using CinemaAbyss.SharedKernel;

namespace CinemaAbyss.Movies.Domain.Entities;

public class Movie : Entity<int>
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public double Rating { get; private set; }
    public List<string> Genres { get; private set; } = new();

    private Movie() { }

    // Validation is left to the database, as in the Go movies microservice.
    public static Movie Create(string title, string description, double rating, IEnumerable<string>? genres = null)
    {
        return new Movie
        {
            Title = title,
            Description = description,
            Rating = rating,
            Genres = genres?.ToList() ?? new List<string>()
        };
    }

    public static Movie Reconstitute(int id, string title, string description, double rating, IEnumerable<string> genres) =>
        new()
        {
            Id = id,
            Title = title,
            Description = description,
            Rating = rating,
            Genres = genres.ToList()
        };
}
