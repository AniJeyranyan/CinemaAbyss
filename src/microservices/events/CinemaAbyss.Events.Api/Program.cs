using System.Text.Json;
using CinemaAbyss.Events.Application;
using CinemaAbyss.Events.Infrastructure;
using CinemaAbyss.Events.Infrastructure.Kafka;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8082";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// The docker-compose variables (KAFKA_BOOTSTRAP_SERVERS, KAFKA_CONSUMER_GROUP) override the "Kafka" section.
builder.Services.PostConfigure<KafkaOptions>(options =>
{
    options.BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS") ?? options.BootstrapServers;
    options.ConsumerGroupId = Environment.GetEnvironmentVariable("KAFKA_CONSUMER_GROUP") ?? options.ConsumerGroupId;
});

var app = builder.Build();

app.MapControllers();

app.Run();
