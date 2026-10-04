using CinemaAbyss.Monolith.Application.Common.Interfaces;
using CinemaAbyss.Monolith.Application.Payments.Queries;
using CinemaAbyss.Monolith.Domain.Entities;
using Moq;
using Xunit;

namespace CinemaAbyss.Monolith.UnitTests.Payments;

public class GetAllPaymentsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnAllPayments_When_NoUserIdGiven()
    {
        var repository = new Mock<IPaymentRepository>();
        repository
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { Payment.Reconstitute(1, 1, 9.99m, DateTime.UtcNow) });

        var handler = new GetAllPaymentsQueryHandler(repository.Object);

        var result = await handler.Handle(new GetAllPaymentsQuery(null), CancellationToken.None);

        Assert.Single(result);
        repository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        repository.Verify(r => r.GetByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_FilterByUserId_When_UserIdGiven()
    {
        var repository = new Mock<IPaymentRepository>();
        repository
            .Setup(r => r.GetByUserIdAsync(42, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { Payment.Reconstitute(1, 42, 14.99m, DateTime.UtcNow) });

        var handler = new GetAllPaymentsQueryHandler(repository.Object);

        var result = await handler.Handle(new GetAllPaymentsQuery(42), CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(42, result[0].UserId);
        repository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
