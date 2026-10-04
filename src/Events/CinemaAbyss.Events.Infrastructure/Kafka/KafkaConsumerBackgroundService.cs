using CinemaAbyss.Events.Domain.Events;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CinemaAbyss.Events.Infrastructure.Kafka;

/// <summary>
/// Demonstrates the event-driven round trip required by the Kafka MVP: the same
/// service that produces movie/user/payment events also consumes and logs them.
/// </summary>
public class KafkaConsumerBackgroundService : BackgroundService
{
    private readonly KafkaOptions _options;
    private readonly ILogger<KafkaConsumerBackgroundService> _logger;

    public KafkaConsumerBackgroundService(IOptions<KafkaOptions> options, ILogger<KafkaConsumerBackgroundService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) =>
        Task.Run(() => Consume(stoppingToken), stoppingToken);

    private void Consume(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _options.BootstrapServers,
            GroupId = _options.ConsumerGroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(new[] { EventTopics.MovieEvents, EventTopics.UserEvents, EventTopics.PaymentEvents });

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    if (result is not null)
                    {
                        _logger.LogInformation(
                            "Consumed event from topic {Topic} partition {Partition} offset {Offset}: {Value}",
                            result.Topic, result.Partition.Value, result.Offset.Value, result.Message.Value);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Error consuming Kafka message.");
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Shutting down.
        }
        finally
        {
            consumer.Close();
        }
    }
}
