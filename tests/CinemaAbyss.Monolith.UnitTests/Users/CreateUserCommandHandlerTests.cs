using CinemaAbyss.Monolith.Application.Common.Interfaces;
using CinemaAbyss.Monolith.Application.Users.Commands;
using CinemaAbyss.Monolith.Domain.Entities;
using Moq;
using Xunit;

namespace CinemaAbyss.Monolith.UnitTests.Users;

public class CreateUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_PersistUser_And_ReturnDto()
    {
        var repository = new Mock<IUserRepository>();
        repository
            .Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User u, CancellationToken _) => User.Reconstitute(1, u.Username, u.Email, u.CreatedAt));

        var handler = new CreateUserCommandHandler(repository.Object);

        var result = await handler.Handle(new CreateUserCommand("john_doe", "john@example.com"), CancellationToken.None);

        Assert.Equal(1, result.Id);
        Assert.Equal("john_doe", result.Username);
        Assert.Equal("john@example.com", result.Email);
        repository.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Persist_BlankUsername_LikeTheGoMonolith()
    {
        // The Go handler validates nothing before INSERT, so a blank username reaches
        // the database rather than being rejected in the application layer.
        var repository = new Mock<IUserRepository>();
        repository
            .Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User u, CancellationToken _) => User.Reconstitute(1, u.Username, u.Email, u.CreatedAt));

        var handler = new CreateUserCommandHandler(repository.Object);

        var result = await handler.Handle(new CreateUserCommand("", "john@example.com"), CancellationToken.None);

        Assert.Equal(string.Empty, result.Username);
        repository.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
