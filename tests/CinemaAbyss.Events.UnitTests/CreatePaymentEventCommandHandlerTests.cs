using CinemaAbyss.Events.Application.Common.Interfaces;
using CinemaAbyss.Events.Application.Common.Models;
using CinemaAbyss.Events.Application.Payments;
using CinemaAbyss.Events.Domain.Events;
using Moq;
using Xunit;

namespace CinemaAbyss.Events.UnitTests;

public class CreatePaymentEventCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_PublishToPaymentEventsTopic_And_ReturnResponse()
    {
        var publisher = new Mock<IEventPublisher>();
        publisher
            .Setup(p => p.PublishAsync(EventTopics.PaymentEvents, It.IsAny<EventEnvelope>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PublishResult(1, 7));

        var handler = new CreatePaymentEventCommandHandler(publisher.Object);

        var command = new CreatePaymentEventCommand(1, 1, 9.99m, "completed", MethodType: "credit_card");
        var response = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("success", response.Status);
        Assert.Equal(1, response.Partition);
        Assert.Equal(7, response.Offset);
        Assert.Equal("payment-1-completed", response.Event.Id);
        Assert.Equal("payment", response.Event.Type);
    }
}
