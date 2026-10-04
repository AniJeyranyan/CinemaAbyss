using CinemaAbyss.Events.Domain.Events;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CinemaAbyss.Events.Infrastructure.Kafka;

/// <summary>
/// Consumes every event topic in the same process that produces them, and logs each message.
/// This is the "the service both produces and consumes" part of the Kafka MVP.
/// </summary>
public sealed class KafkaConsumerBackgroundService : BackgroundService
{
    private readonly KafkaOptions _options;
    private readonly ILogger<KafkaConsumerBackgroundService> _logger;

    public KafkaConsumerBackgroundService(IOptions<KafkaOptions> options, ILogger<KafkaConsumerBackgroundService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) =>
        Task.Factory.StartNew(() => Consume(stoppingToken), stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

    private void Consume(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _options.BootstrapServers,
            GroupId = _options.ConsumerGroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(EventTopics.All);
        _logger.LogInformation("Consuming {Topics} as group {Group}", string.Join(", ", EventTopics.All), _options.ConsumerGroupId);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    if (result?.Message is null) continue;

                    _logger.LogInformation(
                        "[consumer] topic={Topic} partition={Partition} offset={Offset} key={Key} value={Value}",
                        result.Topic, result.Partition.Value, result.Offset.Value, result.Message.Key, result.Message.Value);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Error consuming a Kafka message");
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
