using System.Text.Json;
using CinemaAbyss.Events.Application.Common.Interfaces;
using CinemaAbyss.Events.Application.Common.Models;
using CinemaAbyss.Events.Domain.Events;
using CinemaAbyss.SharedKernel.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace CinemaAbyss.Events.Infrastructure.Kafka;

public class KafkaEventPublisher : IEventPublisher, IDisposable
{
    // The message on the topic uses the same snake_case field names as the HTTP API,
    // so producers and consumers share one wire contract.
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = new SnakeCaseNamingPolicy()
    };

    private readonly IProducer<string, string> _producer;

    public KafkaEventPublisher(IOptions<KafkaOptions> options)
    {
        var config = new ProducerConfig { BootstrapServers = options.Value.BootstrapServers };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task<PublishResult> PublishAsync(string topic, EventEnvelope envelope, CancellationToken cancellationToken)
    {
        var value = JsonSerializer.Serialize(
            new
            {
                id = envelope.Id,
                type = envelope.Type.ToString().ToLowerInvariant(),
                timestamp = envelope.Timestamp,
                payload = envelope.Payload
            },
            SerializerOptions);

        var message = new Message<string, string> { Key = envelope.Id, Value = value };
        var result = await _producer.ProduceAsync(topic, message, cancellationToken);

        return new PublishResult(result.Partition.Value, result.Offset.Value);
    }

    public void Dispose() => _producer.Dispose();
}
