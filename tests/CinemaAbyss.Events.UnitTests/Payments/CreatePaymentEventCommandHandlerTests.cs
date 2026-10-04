using CinemaAbyss.Events.Application.Common.Interfaces;
using CinemaAbyss.Events.Application.Common.Models;
using CinemaAbyss.Events.Application.Payments;
using CinemaAbyss.Events.Domain.Events;
using Moq;

namespace CinemaAbyss.Events.UnitTests.Payments;

public class CreatePaymentEventCommandHandlerTests
{
    [Fact]
    public async Task Handle_PublishesPaymentToPaymentTopicKeyedByPaymentId()
    {
        EventEnvelope? published = null;
        var publisher = new Mock<IEventPublisher>();
        publisher.Setup(p => p.PublishAsync(It.IsAny<EventEnvelope>(), It.IsAny<CancellationToken>()))
            .Callback<EventEnvelope, CancellationToken>((envelope, _) => published = envelope)
            .ReturnsAsync(new PublishResult(2, 10));

        var handler = new CreatePaymentEventCommandHandler(publisher.Object);
        var command = new CreatePaymentEventCommand(
            PaymentId: 4, UserId: 1, Amount: 9.99m, Status: "completed", MethodType: "credit_card",
            Timestamp: DateTimeOffset.UtcNow);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(2, response.Partition);
        Assert.Equal(10, response.Offset);
        Assert.Equal("payment", response.Event.Type);
        Assert.Equal("payment-events", published!.Topic);
        Assert.Equal("4", published.Key);
        Assert.Equal(new PaymentEventPayload(4, 1, 9.99m, "completed", "credit_card"), published.Payload);
    }
}
