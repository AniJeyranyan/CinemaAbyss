using System.Text.Json;
using CinemaAbyss.Events.Application.Common.Interfaces;
using CinemaAbyss.Events.Application.Common.Models;
using CinemaAbyss.Events.Domain.Events;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace CinemaAbyss.Events.Infrastructure.Kafka;

public sealed class KafkaEventPublisher : IEventPublisher, IDisposable
{
    private readonly IProducer<string, string> _producer;

    public KafkaEventPublisher(IOptions<KafkaOptions> options)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
            Acks = Acks.All,
            MessageTimeoutMs = 10_000
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task<PublishResult> PublishAsync(EventEnvelope envelope, CancellationToken cancellationToken)
    {
        var message = new KafkaMessage(
            envelope.Id,
            envelope.Type.ToString().ToLowerInvariant(),
            envelope.Timestamp,
            envelope.Payload);

        var value = JsonSerializer.Serialize(message, KafkaMessage.JsonOptions);
        var result = await _producer.ProduceAsync(
            envelope.Topic,
            new Message<string, string> { Key = envelope.Key, Value = value },
            cancellationToken);

        return new PublishResult(result.Partition.Value, result.Offset.Value);
    }

    public void Dispose() => _producer.Dispose();
}
