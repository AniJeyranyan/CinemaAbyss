using CinemaAbyss.Movies.Application.Common.Interfaces;
using CinemaAbyss.Movies.Application.Movies.Queries;
using CinemaAbyss.Movies.Domain.Entities;
using Moq;
using Xunit;

namespace CinemaAbyss.Movies.UnitTests;

public class GetMovieByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnNull_When_MovieNotFound()
    {
        var repository = new Mock<IMovieRepository>();
        repository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((Movie?)null);

        var handler = new GetMovieByIdQueryHandler(repository.Object);

        var result = await handler.Handle(new GetMovieByIdQuery(99), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_Should_ReturnDto_When_MovieFound()
    {
        var repository = new Mock<IMovieRepository>();
        repository
            .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Movie.Reconstitute(1, "Inception", "desc", 8.8, new[] { "Sci-Fi" }));

        var handler = new GetMovieByIdQueryHandler(repository.Object);

        var result = await handler.Handle(new GetMovieByIdQuery(1), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Inception", result!.Title);
    }
}
