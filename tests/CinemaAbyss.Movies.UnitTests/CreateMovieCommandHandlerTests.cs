using CinemaAbyss.Movies.Application.Common.Interfaces;
using CinemaAbyss.Movies.Application.Movies.Commands;
using CinemaAbyss.Movies.Domain.Entities;
using Moq;
using Xunit;

namespace CinemaAbyss.Movies.UnitTests;

public class CreateMovieCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_PersistMovie_And_ReturnDto_With_Genres()
    {
        var repository = new Mock<IMovieRepository>();
        repository
            .Setup(r => r.AddAsync(It.IsAny<Movie>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Movie m, CancellationToken _) => Movie.Reconstitute(1, m.Title, m.Description, m.Rating, m.Genres));

        var handler = new CreateMovieCommandHandler(repository.Object);

        var result = await handler.Handle(
            new CreateMovieCommand("Inception", "A mind-bending thriller", 8.8, new[] { "Sci-Fi", "Action" }),
            CancellationToken.None);

        Assert.Equal(1, result.Id);
        Assert.Equal("Inception", result.Title);
        Assert.Equal(new[] { "Sci-Fi", "Action" }, result.Genres);
    }

    [Fact]
    public async Task Handle_Should_Persist_OutOfRangeRating_LikeTheGoService()
    {
        // The Go handler validates nothing before INSERT, so the rating is stored as sent.
        var repository = new Mock<IMovieRepository>();
        repository
            .Setup(r => r.AddAsync(It.IsAny<Movie>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Movie m, CancellationToken _) => Movie.Reconstitute(1, m.Title, m.Description, m.Rating, m.Genres));

        var handler = new CreateMovieCommandHandler(repository.Object);

        var result = await handler.Handle(
            new CreateMovieCommand("Bad", "desc", 11, Array.Empty<string>()), CancellationToken.None);

        Assert.Equal(11, result.Rating);
    }
}
