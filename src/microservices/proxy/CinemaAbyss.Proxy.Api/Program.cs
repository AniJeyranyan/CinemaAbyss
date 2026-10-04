using CinemaAbyss.Proxy.Api.Endpoints;
using CinemaAbyss.Proxy.Application;
using CinemaAbyss.Proxy.Infrastructure;
using CinemaAbyss.Proxy.Infrastructure.Options;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// The docker-compose variables (MONOLITH_URL, GRADUAL_MIGRATION, ...) override the "Proxy" section.
builder.Services.PostConfigure<ProxyOptions>(options =>
{
    options.MonolithUrl = Environment.GetEnvironmentVariable("MONOLITH_URL") ?? options.MonolithUrl;
    options.MoviesServiceUrl = Environment.GetEnvironmentVariable("MOVIES_SERVICE_URL") ?? options.MoviesServiceUrl;
    options.EventsServiceUrl = Environment.GetEnvironmentVariable("EVENTS_SERVICE_URL") ?? options.EventsServiceUrl;
    options.GradualMigration = bool.TryParse(Environment.GetEnvironmentVariable("GRADUAL_MIGRATION"), out var gradual)
        ? gradual
        : options.GradualMigration;
    options.MoviesMigrationPercent = int.TryParse(Environment.GetEnvironmentVariable("MOVIES_MIGRATION_PERCENT"), out var percent)
        ? percent
        : options.MoviesMigrationPercent;
});

var app = builder.Build();

app.MapProxyEndpoints();

app.Run();
